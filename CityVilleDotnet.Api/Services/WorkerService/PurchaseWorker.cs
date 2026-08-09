using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Api.Services.WorkerService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorkerService;

public class PurchaseWorker(CityVilleDbContext context) : AmfService<PurchaseWorkerRequest>
{
    public override async Task<ASObject> HandlePacket(PurchaseWorkerRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (request.Feature == WorkerBucket.TrainsFeature)
            return await PurchaseTrainStop(request, playerId, cancellationToken);

        if (request.Feature != WorkerBucket.FactoriesFeature)
            throw new Exception($"Unsupported worker feature {request.Feature}");

        var objectId = WorkerBucket.ParseObjectId(request.Bucket);

        var player = await context.Set<Player>()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == objectId || o.TempId == objectId))
            .ThenInclude(x => x.Workers)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var building = player.GetWorld().GetBuildingByClientId(objectId) ?? throw new Exception("Target building not found");

        if (building.ContractName is null)
            throw new Exception("No contract started on this factory");

        var contractItem = GameSettingsManager.Instance.GetItem(building.ContractName) ?? throw new Exception("Contract item not found");

        if (contractItem.Workers is null)
            throw new Exception($"No workers defined on contract {building.ContractName}");

        player.RemoveCash(contractItem.Workers.CashCost);
        building.AddPurchasedWorker();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }

    private async Task<ASObject> PurchaseTrainStop(PurchaseWorkerRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (request.Bucket != WorkerBucket.TrainsBucket)
            throw new Exception($"Invalid worker bucket {request.Bucket}");

        var player = await context.Set<Player>()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.TrainOrder)
            .ThenInclude(x => x!.Workers)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var order = player.GetWorld().TrainOrder ?? throw new Exception("No train order in progress");

        player.RemoveCash(order.GetStopCashCost());
        order.AddPurchasedStop();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class PurchaseWorkerRequest
{
    [AmfParam(0)] public string Feature { get; set; } = string.Empty;
    [AmfParam(1)] public string Bucket { get; set; } = string.Empty;
}

public class PurchaseWorkerValidator : AbstractValidator<PurchaseWorkerRequest>
{
    public PurchaseWorkerValidator()
    {
        RuleFor(x => x.Feature).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Bucket).NotEmpty().MaximumLength(16);
    }
}
