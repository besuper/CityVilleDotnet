using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CityVilleDotnet.Api.Services.UserService;

public class PurchaseCrewMember(CityVilleDbContext context, ILogger<PurchaseCrewMember> logger) : AmfService<PurchaseCrewMemberRequest>
{
    public override async Task<ASObject> HandlePacket(PurchaseCrewMemberRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        logger.LogDebug("Purchasing crew member for {RequestObjectId} for gate {RequestGateName}", request.ObjectId, request.GateName);

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .ThenInclude(x => x.CrewMembers)
            .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();
        var building = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception("Target building not found");

        var gameItem = GameSettingsManager.Instance.GetItem(building.GetItemName()) ?? throw new Exception("Game item not found");

        var gates = gameItem.GetGates();

        if (gates.Count == 0) throw new Exception("No gates defined in this building");

        var targetGate = (request.GateName.IsNullOrEmpty() ? gates.FirstOrDefault() : gates.FirstOrDefault(x => x.Name == request.GateName)) ?? throw new Exception($"Can't find target gate {request.GateName}");

        var key = targetGate.Keys.FirstOrDefault(x => x?.Name == "required_crew") ?? throw new Exception("Can't find required key");

        if (key.CashCost is null)
            throw new Exception("Cash cost is null on key");

        player.RemoveCash(key.CashCost.Value);
        building.AddCrewMember(null);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class PurchaseCrewMemberRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(1)] public string? GateName { get; set; }
}

public class PurchaseCrewMemberValidator : AbstractValidator<PurchaseCrewMemberRequest>
{
    public PurchaseCrewMemberValidator()
    {
        RuleFor(x => x.GateName).MaximumLength(32);
    }
}