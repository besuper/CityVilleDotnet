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
            logger.LogInformation($"{item.Key} = {item.Value}");
        }

        // TODO: Implement components
        // ignore components for now

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var className = (string)building["className"];
        var itemName = (string)building["itemName"];
        var buildTime = building.GetValueOrDefault("buildTime");
        var plantTime = building.GetValueOrDefault("plantTime");
        var world = user.GetWorld();

        var obj = new WorldObject(
            itemName,
            className,
            null,
            (bool)building["deleted"],
            (int)building["tempId"],
            (string)building["state"],
            (int)building["direction"],
            buildTime == null ? 0 : (double)buildTime,
            plantTime == null ? 0 : (double)plantTime,
            new WorldObjectPosition()
            {
                X = (int)position["x"],
                Y = (int)position["y"],
                Z = (int)position["z"]
            },
            (int)building["id"]
        );

        logger.LogInformation($"x: {obj.Position.X} y: {obj.Position.Y} z: {obj.Position.Z}");

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is not null)
        {
            if (gameItem.Cost is not null)
                user.RemoveCoin(gameItem.Cost.Value);

            if (gameItem.Construction is not null)
                obj.SetAsConstructionSite(gameItem.Construction);
        }

        world.AddBuilding(obj);

        // TODO: Check coins, goods, energy, etc...
        // Add population

        user.HandleQuestProgress();
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);
    }
}
