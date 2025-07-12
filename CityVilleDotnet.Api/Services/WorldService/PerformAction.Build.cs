using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task PerformBuild(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null when action type is place");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");

        var obj = user.GetWorld().GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]) ?? throw new Exception($"Can't find building");

        if (obj.Builds is null)
            throw new Exception($"Can't find `builds`");
        
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);
        
        if(gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");
        
        if(gameItem.NumberOfStages is null)
            throw new Exception($"Game item {obj.ItemName} doesn't have number of stages defined");

        obj.AddConstructionStage();

        if (obj.Stage != gameItem.NumberOfStages)
        {
            user.CollectDoobersRewards(obj.ItemName, ["xp"]);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}