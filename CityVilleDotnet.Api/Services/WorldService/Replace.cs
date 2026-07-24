using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class Replace(CityVilleDbContext context) : AmfService<ReplaceRequest>
{
    public override async Task<ASObject> HandlePacket(ReplaceRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.Building.Id || o.TempId == request.Building.Id))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken)
            ?? throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.Building.Id)
            ?? throw new Exception($"Can't find object with id {request.Building.Id}");

        var gameItem = GameSettingsManager.Instance.GetItem(request.Building.ItemName)
            ?? throw new Exception($"Item {request.Building.ItemName} not found");

        var className = Enum.Parse<BuildingClassType>(gameItem.Type.Pascalize());

        obj.ReplaceWith(
            request.Building.ItemName,
            className,
            request.Building.Direction,
            request.Building.Position.X,
            request.Building.Position.Y,
            request.Building.Position.Z,
            request.Building.State
        );

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class ReplaceRequest
{
    [AmfParam(1)] public ReplaceSaveObject Building { get; set; } = new();
}

public class ReplaceSaveObject
{
    [AmfParam("id")] public int Id { get; set; }
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
    [AmfParam("direction")] public int Direction { get; set; }
    [AmfParam("itemName")] public string ItemName { get; set; } = string.Empty;
    [AmfParam("state")] public WorldObjectState State { get; set; }
}
