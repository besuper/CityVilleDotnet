using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class SendToInventory(CityVilleDbContext context) : AmfService<SendToInventoryRequest>
{
    public override async Task<ASObject> HandlePacket(SendToInventoryRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null) throw new Exception($"User not found with id {userId}");

        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception($"Can't find building at {request.Building.Position}");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.GetItemName());

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.GetItemName()}");

        if (gameItem.SellSendsToInventory is null)
            throw new Exception("SellSendsToInventory is null");

        if (bool.TryParse(gameItem.SellSendsToInventory, out var result) && !result)
            throw new Exception("SellSendsToInventory is disabled");

        user.Player!.AddItem(obj.GetItemName());

        world.RemoveBuilding(obj);
        context.Set<WorldObject>().Remove(obj);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class SendToInventoryRequest
{
    [AmfParam(1)] public BuildingSendToInventoryRequest Building { get; set; } = new();
}

public class BuildingSendToInventoryRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}