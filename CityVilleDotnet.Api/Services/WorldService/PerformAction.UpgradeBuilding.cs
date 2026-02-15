using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> UpgradeBuilding(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null when action type is upgradeBuilding");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(Convert.ToInt32(position["x"]), Convert.ToInt32(position["y"]), Convert.ToInt32(position["z"])) ?? throw new Exception("Can't find building");
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.Upgrade is null)
            throw new Exception("The game item doesn't contains upgrade item");

        // TODO: Check if crew members are purchased
        // TODO: Check requirements (population, ...)
        
        var newItemName = gameItem.Upgrade.Name;

        if (gameItem.Upgrade.CashCost is not null)
        {
            user.Player?.RemoveCash(Convert.ToInt32(gameItem.Upgrade.CashCost));
        }

        obj.UpgradeBuilding(newItemName);
        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}