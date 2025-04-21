using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task PerformPlace(User user, object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var building = _params[1] as ASObject;

        if (building is null)
        {
            throw new Exception("Building can't be null when action type is place");
        }

        foreach (var item in building)
        {
            _logger.LogInformation($"{item.Key} = {item.Value}");
        }

        // TODO: Implement components
        // ignore components for now

        var position = building["position"] as ASObject;
        var className = (string)building["className"];
        var itemName = (string)building["itemName"];
        var world = user.GetWorld();

        var obj = new WorldObject(
            itemName,
            className,
            null,
            (bool)building["deleted"],
            (int)building["tempId"],
            (string)building["state"],
            (int)building["direction"],
            (double)building["buildTime"],
            (double)building["plantTime"],
            new WorldObjectPosition()
            {
                X = (int)position["x"],
                Y = (int)position["y"],
                Z = (int)position["z"]
            },
            (int)building["id"]
        );

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is not null)
        {
            if (gameItem.Cost is not null)
            {
                user.RemoveCoin(gameItem.Cost.Value);
            }

            if (gameItem.Construction is not null)
            {
                obj.SetAsConstructionSite(gameItem.Construction);
            }
        }

        world.AddBuilding(obj);

        // TODO: Check coins, goods, energy, etc...
        // Add population

        await _context.SaveChangesAsync(cancellationToken);
    }
}
