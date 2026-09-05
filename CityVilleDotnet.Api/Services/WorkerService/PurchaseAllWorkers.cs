using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Api.Services.WorkerService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorkerService;

public class PurchaseAllWorkers(CityVilleDbContext context) : AmfService<PurchaseAllWorkersRequest>
{
    public override async Task<ASObject> HandlePacket(PurchaseAllWorkersRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (request.Feature != WorkerBucket.FactoriesFeature)
            throw new Exception($"Unsupported worker feature {request.Feature}");

        var objectId = WorkerBucket.ParseObjectId(request.Bucket);

        var player = await context.Set<Player>()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == objectId || o.TempId == objectId))
            .ThenInclude(x => x.Workers)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var building = player.GetWorld().GetBuildingByClientId(objectId) ?? throw new Exception("Target building not found");

        if (building.ContractName is null)
            throw new Exception("No contract started on this factory");

        var contractItem = GameSettingsManager.Instance.GetItem(building.ContractName) ?? throw new Exception("Contract item not found");

        if (contractItem.Workers is null)
            throw new Exception($"No workers defined on contract {building.ContractName}");

        var remainingSpots = building.GetRemainingWorkerSpots();

        if (remainingSpots <= 0)
            throw new Exception($"No worker spot left on object {building.WorldFlatId}");

        player.RemoveCash(CalculateInstantCashCost(contractItem, remainingSpots));

        for (var i = 0; i < remainingSpots; i++)
        {
            building.AddPurchasedWorker();
        }

        // client instantly finishes the contract when buying all workers (Factory::purchaseAllWorkers)
        building.SetReadyToHarvest();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }

    // Factory::calculateInstantCashCost
    private static int CalculateInstantCashCost(GameItem contractItem, int remainingSpots)
    {
        var growTimeMinutes = (int)Math.Round((contractItem.GrowTime ?? 0) * 23 * 60);
        var workersCost = remainingSpots * contractItem.Workers!.CashCost;

        return growTimeMinutes switch
        {
            30 => workersCost + 2,
            120 => workersCost + 5,
            720 => workersCost + 10,
            1440 => workersCost + 15,
            3024 => workersCost + 50,
            _ => 50
        };
    }
}

public class PurchaseAllWorkersRequest
{
    [AmfParam(0)] public string Feature { get; set; } = string.Empty;
    [AmfParam(1)] public string Bucket { get; set; } = string.Empty;
}

public class PurchaseAllWorkersValidator : AbstractValidator<PurchaseAllWorkersRequest>
{
    public PurchaseAllWorkersValidator()
    {
        RuleFor(x => x.Feature).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Bucket).NotEmpty().MaximumLength(16);
    }
}
