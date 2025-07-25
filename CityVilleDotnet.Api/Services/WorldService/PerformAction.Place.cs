using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task PerformPlace(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null when action type is place");

        foreach (var item in building)
        {
            logger.LogInformation("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        // TODO: Implement components
        // ignore components for now

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var className = (string)building["className"];
        var itemName = (string)building["itemName"];
        var buildTime = building.GetValueOrDefault("buildTime");
        var plantTime = building.GetValueOrDefault("plantTime");
        var world = user.GetWorld();

        var newId = world.GetAvailableBuildingId();

        logger.LogInformation("Using new ID {NewId}", newId);

        var obj = new WorldObject(
            itemName,
            className,
            null,
            (bool)building["deleted"],
            (int)building["tempId"],
            (string)building["state"],
            (int)building["direction"],
            (double?)buildTime,
            (double?)plantTime,
            (int)position["x"],
            (int)position["y"],
            (int)position["z"], // TODO: Remove Z coordinate, seems not used in CityVille
            newId
        );

        logger.LogInformation("x: {PositionX} y: {PositionY} z: {PositionZ}", obj.X, obj.Y, obj.Z);

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is not null)
        {
            if (gameItem.Cost is not null)
                user.Player!.RemoveCoins(gameItem.Cost.Value);

            if (gameItem.Construction is not null)
            {
                obj.SetAsConstructionSite(gameItem.Construction);
            }
        }

        world.AddBuilding(obj);

        if (user.Player!.HasItem(itemName))
        {
            var removedItem = user.Player.RemoveItem(itemName);

            if (removedItem is not null)
                context.Set<InventoryItem>().Remove(removedItem);
        }

        // TODO: Check coins, goods, energy, etc...
        // Add population

        user.HandleQuestsProgress("placeByClass", className: obj.ClassName);
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);
    }
}