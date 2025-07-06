using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse?> PerformOpenBusiness(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception($"Building can't be null");

        foreach (var item in building)
        {
            logger.LogInformation($"{item.Key} = {item.Value}");
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]) ?? throw new Exception($"Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is not null)
        {
            if (gameItem.CommodityRequired is not null)
            {
                if (user.Player.Goods < gameItem.CommodityRequired)
                    // TODO: Show an error ?
                    return new CityVilleResponse(9, 333);

                user.RemoveGoods(gameItem.CommodityRequired.Value);

                obj.BuildTime = (double)building["buildTime"];
                obj.PlantTime = (double)building["plantTime"];
                obj.State = (string)building["state"];

                user.HandleQuestProgress();
                user.CheckCompletedQuests();
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        return null;
    }
}
