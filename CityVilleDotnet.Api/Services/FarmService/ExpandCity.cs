using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FarmService;

public class ExpandCity(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
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

        var itemName = (string)@params[0]; // expand_12_12
        var item = GameSettingsManager.Instance.GetItem(itemName);

        if (item is null)
            throw new Exception($"Can't find item {itemName}");

        if (item.Height is null || item.Width is null)
            throw new Exception($"Item {itemName} has no height or width defined");

        var permitData = user.Player.GetExpansionData();

        if (permitData is null) throw new Exception("Can't find permit data");

        var requiredPermit = permitData[1];
        var permitName = item.Unlock ?? "";

        if (user.Player.CountInventoryItem(permitName) < requiredPermit)
        {
            throw new Exception($"You need {requiredPermit} {permitName} to expand this city");
        }

        var coordinates = (ASObject)@params[1];
        var x = (int?)coordinates["x"];
        var y = (int?)coordinates["y"];
        var weight = item.Height;
        var width = item.Width;

        if (x is null || y is null || weight is null || width is null)
            throw new Exception("Can't find MapRect coordinates");

        var world = user.GetWorld();

        // Add the new map area

        var newMapRect = new MapRect
        {
            Id = Guid.NewGuid(),
            X = x.Value,
            Y = y.Value,
            Height = int.Parse(item.Height), // FIXME: Change these value to the right type when loading the settings
            Width = int.Parse(item.Width)
        };

        world.AddMapRect(newMapRect);

        // Add new trees

        var trees = (object[])@params[2];
        var remapedIds = new List<object>();

        foreach (ASObject tree in trees)
        {
            var newTree = new WorldObject
            {
                Id = Guid.NewGuid(),
                WorldFlatId = world.GetAvailableBuildingId(),
                TempId = -1,
                ItemName = (string)tree["itemName"],
                ClassName = "Wilderness",
                State = WorldObjectState.Static,
                Direction = (int)tree["dir"],
                Deleted = false,
                X = (int)tree["x"],
                Y = (int)tree["y"]
            };

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