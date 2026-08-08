using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class MakeTypeInstantReady(CityVilleDbContext context, ILogger<MakeTypeInstantReady> logger) : AmfService<MakeTypeInstantReadyRequest>
{
    private static readonly Dictionary<string, BuildingClassType> TypeToWorldObjType = new()
    {
        { "plot", BuildingClassType.Plot },
        { "ship", BuildingClassType.Ship },
        { "harvestableShip", BuildingClassType.HarvestableShip },
        { "residence", BuildingClassType.Residence },
        { "business", BuildingClassType.Business },
        { "airplane", BuildingClassType.Airplane },
        { "helicopter", BuildingClassType.Heliport },
    };

    public override async Task<ASObject> HandlePacket(MakeTypeInstantReadyRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (!TypeToWorldObjType.TryGetValue(request.BuildingType, out var buildingType))
            throw new Exception($"Building type not supported {request.BuildingType}");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.ClassName == buildingType && o.State == WorldObjectState.Planted))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var cost = 0;

        foreach (var obj in world.Objects)
        {
            if (obj.HasGrown()) continue;

            cost += obj.GetCostToMakeReady();
        }

        // FIXME: Cost is not the same as on the client side
        cost = Math.Max(cost, 1);

        logger.LogDebug("Bought instant finish for type {BuildingType} cost {Cost}", buildingType, cost);

        if (player.Cash < cost)
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        player.RemoveCash(cost);

        foreach (var obj in world.Objects)
        {
            if (obj.HasGrown()) continue;

            obj.SetReadyToHarvest();
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class MakeTypeInstantReadyRequest
{
    [AmfParam(0)] public string BuildingType { get; set; } = string.Empty;
}

public class MakeTypeInstantReadyValidator : AbstractValidator<MakeTypeInstantReadyRequest>
{
    public MakeTypeInstantReadyValidator()
    {
        RuleFor(x => x.BuildingType).NotEmpty().MaximumLength(32);
    }
}