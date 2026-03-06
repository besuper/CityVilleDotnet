using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FarmService;

// TODO: Rework this transaction
public class ExpandCity(CityVilleDbContext context) : AmfService<ExpandCityRequest>
{
    public override async Task<ASObject> HandlePacket(ExpandCityRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null)
            throw new Exception("Can't find user");

        var item = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (item is null)
            throw new Exception($"Can't find item {request.ItemName}");

        if (item.Height is null || item.Width is null)
            throw new Exception($"Item {request.ItemName} has no height or width defined");

        var permitData = user.Player.GetExpansionData();

        if (permitData is null) throw new Exception("Can't find permit data");

        var requiredPermit = permitData[1];
        var permitName = item.Unlock ?? "";

        if (user.Player.CountInventoryItem(permitName) < requiredPermit)
        {
            throw new Exception($"You need {requiredPermit} {permitName} to expand this city");
        }

        var world = user.GetWorld();

        // Add the new map area

        var newMapRect = new MapRect
        {
            X = request.Coordinates.X,
            Y = request.Coordinates.Y,
            Height = int.Parse(item.Height), // FIXME: Change these value to the right type when loading the settings
            Width = int.Parse(item.Width)
        };

        world.AddMapRect(newMapRect);

        // Add new trees

        var remapedIds = new List<object>();

        foreach (ASObject tree in request.Trees)
        {
            var newTree = new WorldObject(
                (string)tree["itemName"],
                BuildingClassType.Wilderness,
                null,
                false,
                -1,
                WorldObjectState.Static,
                0,
                ServerUtils.GetCurrentTime(),
                ServerUtils.GetCurrentTime(),
                (int)tree["x"],
                (int)tree["y"],
                0,
                world.GetAvailableBuildingId()
            );

            world.AddBuilding(newTree);

            remapedIds.Add(new
            {
                id = (int)tree["id"],
                newId = newTree.WorldFlatId
            });
        }

        user.Player.IncrementExpansionsPurchased();
        var removedItem = user.Player.RemoveItem(permitName, requiredPermit);

        if (removedItem is not null)
            context.Set<InventoryItem>().Remove(removedItem);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(remapedIds);
    }
}

public sealed class ExpandCityRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public ExpandCityCoordinates Coordinates { get; set; } = new();
    [AmfParam(2)] public object[] Trees { get; set; } = [];
}

public class ExpandCityCoordinates
{
    [AmfParam("x")] public int X { get; set; }
    [AmfParam("y")] public int Y { get; set; }
}