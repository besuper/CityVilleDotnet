using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformHarvest(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]) ?? throw new Exception("Can't find building");

        var coinYield = obj.Harvest();
        var secureRands = user.CollectDoobersRewards(obj.ContractName ?? obj.ItemName);

        logger.LogDebug("Secure rands {Join}", string.Join(",", secureRands.ToArray()));
        logger.LogDebug("Secure rands {SecureRandsCount}", secureRands.Count);

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