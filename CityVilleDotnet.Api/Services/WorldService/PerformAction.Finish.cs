using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformFinish(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var itemId = Convert.ToInt32(building["id"]);
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(Convert.ToInt32(position["x"]), Convert.ToInt32(position["y"]), Convert.ToInt32(position["z"])) ?? throw new Exception($"Can't find building with ID {itemId}");

        if (obj.Builds is null)
            throw new Exception($"Can't find `builds` {obj}");

        user.Player!.CollectDoobersRewards(obj.ItemName);

        obj.FinishConstruction();

        world.CalculateCurrentPopulation();
        world.CalculatePopulationCap();

        user.HandleQuestsProgress(""); // Empty actionType to force recheck counts
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().MetaData(CreateQuestComponentResponse(user)).Data(new ASObject
        {
            ["id"] = obj.WorldFlatId
        });
    }
}
