using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class BuildFloor(CityVilleDbContext context) : AmfService<BuildFloorRequest>
{
    public override async Task<ASObject> HandlePacket(BuildFloorRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        // TODO: Check if this transaction is only used for headquarters
        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.ClassName == request.Building.ClassName))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null) throw new Exception("Player not found");

        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z);

        if (obj is null) throw new Exception("Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.GetItemName());

        if (gameItem is null) throw new Exception($"Can't find item with name {obj.GetItemName()}");

        obj.UpgradeHeadquarterFloor();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class BuildFloorRequest
{
    [AmfParam(1)] public BuildFloorBuildingRequest Building { get; set; } = new();
}

public class BuildFloorBuildingRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
    [AmfParam("className")] public BuildingClassType ClassName { get; set; } = new();
}