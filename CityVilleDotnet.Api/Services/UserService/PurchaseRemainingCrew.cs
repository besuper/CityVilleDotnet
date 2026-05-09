using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CityVilleDotnet.Api.Services.UserService;

public class PurchaseRemainingCrew(CityVilleDbContext context, ILogger<PurchaseCrewMember> logger) : AmfService<PurchaseRemainingCrewRequest>
{
    public override async Task<ASObject> HandlePacket(PurchaseRemainingCrewRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.ObjectId))
            .ThenInclude(x => x.CrewMembers)
            .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();
        var building = world.GetBuildingById(request.ObjectId) ?? throw new Exception("Target building not found");

        var gameItem = GameSettingsManager.Instance.GetItem(building.GetItemName()) ?? throw new Exception("Game item not found");

        var gates = gameItem.GetGates();

        if (gates.Count == 0) throw new Exception("No gates defined in this building");

        var targetGate = (request.GateName.IsNullOrEmpty() ? gates.FirstOrDefault() : gates.FirstOrDefault(x => x.Name == request.GateName)) ?? throw new Exception($"Can't find target gate {request.GateName}");

        var key = targetGate.Keys.FirstOrDefault(x => x?.Name == "required_crew") ?? throw new Exception("Can't find required key");

        if (key.CashCost is null)
            throw new Exception("Cash cost is null on key");

        var remainingCrew = key.Amount - building.CrewMembers.Count;

        if (remainingCrew <= 0) throw new Exception("Crew is full");

        var totalCost = key.CashCost.Value * remainingCrew;

        if (totalCost > player.Cash)
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        logger.LogDebug("Purchased remaining crew for {RequestObjectId} for gate {RequestGateName} for {totalCost} cash", request.ObjectId, request.GateName, totalCost);
        
        player.RemoveCash(totalCost);

        for (var i = 0; i < remainingCrew; i++)
        {
            building.AddCrewMember(null);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class PurchaseRemainingCrewRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(1)] public string? GateName { get; set; }
}

public class PurchaseRemainingCrewValidator : AbstractValidator<PurchaseRemainingCrewRequest>
{
    public PurchaseRemainingCrewValidator()
    {
        RuleFor(x => x.GateName).MaximumLength(32);
    }
}