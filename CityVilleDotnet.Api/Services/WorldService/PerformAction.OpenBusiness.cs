using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse?> PerformOpenBusiness(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]) ?? throw new Exception($"Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item with name {obj.ItemName}");

        if (gameItem.CommodityRequired is null)
            throw new Exception($"Game item {obj.ItemName} doesn't have commodity required");

        if (user.Player!.Goods < gameItem.CommodityRequired)
            // TODO: Show an error ?
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        if (gameItem.EnergyCost?.Open is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Open);

            if (!user.Player!.RemoveEnergy(energyCost))
            {
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
            }
        }

        user.Player.RemoveGoods(gameItem.CommodityRequired.Value);

        obj.BuildTime = (double)building["buildTime"];
        obj.PlantTime = (double)building["plantTime"];
        obj.State = (string)building["state"];

        user.HandleQuestProgress();
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return null;
    }
}