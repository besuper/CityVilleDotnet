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
    public override async Task<ASObject> HandlePacket(SellRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null) throw new Exception($"User not found with id {userId}");

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
                user.Player!.AddItem(obj.ItemName);
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