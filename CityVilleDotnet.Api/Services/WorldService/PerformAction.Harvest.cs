using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformHarvest(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception($"Building can't be null");

        // TODO: Remove (or make debug)
        foreach (var item in building)
        {
            logger.LogInformation($"{item.Key} = {item.Value}");
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]) ?? throw new Exception($"Can't find building");

        var className = (string)building["className"];
        var coinYield = 0;

        if (className == "Plot")
        {
            var contract = (string)building["contractName"];
            var gameItem = GameSettingsManager.Instance.GetItem(contract);

            if (gameItem is not null)
                coinYield = gameItem.CoinYield ?? 0;

            obj.State = "plowed";
        }
        else
        {
            var itemName = (string)building["itemName"];
            var gameItem = GameSettingsManager.Instance.GetItem(itemName);

            if (gameItem is not null)
                coinYield = gameItem.CoinYield ?? 0;
        }

        if (obj.HasGrown())
            obj.SetReadyToHarvest();

        if (obj.State == "open")
            obj.State = "closed";

        if (obj.State == "grown")
        {
            // FIXME: Re-use gameItem ?
            var itemName = (string)building["itemName"];
            var gameItem = GameSettingsManager.Instance.GetItem(itemName);

            if (gameItem is not null)
            {
                var multiplier = Math.Round((double)GameSettingsManager.Instance.GetInt("InGameDaySeconds") * 1000.0 * gameItem.GrowTime ?? 1.0, 2);

                obj.State = "planted";
                obj.PlantTime = ServerUtils.GetCurrentTime() + multiplier;
            }
        }

        var secureRands = user.CollectDoobersRewards(obj.ItemName);

        logger.LogInformation($"Secure rands {secureRands}");
        logger.LogInformation($"Secure rands {secureRands.Count}");

        user.HandleQuestProgress(itemName: obj.ItemName == "plot_crop" ? obj.ClassName : obj.ItemName);
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse(333, new ASObject
        {
            ["retCoinYield"] = coinYield,
            //response["doobers"] = AmfConverter.Convert(new List<int>());
            ["secureRands"] = AmfConverter.Convert(secureRands)
        });
    }
}