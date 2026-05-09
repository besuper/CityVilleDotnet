using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Sell(CityVilleDbContext context) : AmfService<SellRequest>
{
    public override async Task<ASObject> HandlePacket(SellRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.X == request.Building.Position.X && o.Y == request.Building.Position.Y))
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null) throw new Exception("Player not found");

        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z);

        if (obj is null) throw new Exception("Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null) throw new Exception($"Can't find item with name {obj.ItemName}");

        world.RemoveBuilding(obj);
        context.Set<WorldObject>().Remove(obj);

        if (gameItem.SellSendsToInventory is not null)
        {
            if (bool.TryParse(gameItem.SellSendsToInventory, out var result) && result)
            {
                user.AddItem(obj.ItemName);
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class SellRequest
{
    [AmfParam(1)] public SellBuildingRequest Building { get; set; } = new();
}

public class SellBuildingRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}