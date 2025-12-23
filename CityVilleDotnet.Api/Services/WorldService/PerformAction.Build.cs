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

        var obj = user.GetWorld().GetBuildingByCoord(Convert.ToInt32(position["x"]), Convert.ToInt32(position["y"]), Convert.ToInt32(position["z"])) ?? throw new Exception($"Can't find building");

        if (obj.Builds is null)
            throw new Exception($"Can't find `builds`");
        
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);
        
        if(gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");
        
        if(gameItem.NumberOfStages is null)
            throw new Exception($"Game item {obj.ItemName} doesn't have number of stages defined");

        if (gameItem.EnergyCost?.Build is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Build);

            if (!user.Player!.RemoveEnergy(energyCost))
            {
                // FIXME: Return error response
                return;
            }
        }else if (gameItem.EnergyCostPerBuild is not null)
        {
            if (!user.Player!.RemoveEnergy(gameItem.EnergyCostPerBuild.Value))
            {
                // FIXME: Return error response
                return;
            }
        } 

        obj.AddConstructionStage();

        if (obj.Stage != gameItem.NumberOfStages)
        {
            user.Player!.CollectDoobersRewards(obj.ItemName, ["xp"]);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}