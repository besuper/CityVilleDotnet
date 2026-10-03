using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class RollUpgradeItemName(CityVilleDbContext context) : AmfService<RollUpgradeItemNameRequest>
{
    public override async Task<ASObject> HandlePacket(RollUpgradeItemNameRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.TempId == request.Building.Id || o.WorldFlatId == request.Building.Id || o.ClassName == BuildingClassType.Municipal))
            .Include(x => x.FeatureRollCounters)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.Building.Id);

        player.RollUpgradeItemName(obj);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class RollUpgradeItemNameRequest
{
    [AmfParam(1)] public RollUpgradeItemNameBuildingRequest Building { get; set; } = new();
}

public class RollUpgradeItemNameBuildingRequest
{
    [AmfParam("id")] public int Id { get; set; }
}
