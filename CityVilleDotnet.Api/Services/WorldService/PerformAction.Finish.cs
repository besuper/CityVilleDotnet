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
            logger.LogInformation("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var itemId = (int)building["id"];
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]) ?? throw new Exception($"Can't find building with ID {itemId}");

        if (obj.Builds is null)
            throw new Exception($"Can't find `builds` {obj}");

        user.CollectDoobersRewards(obj.ItemName);

        obj.FinishConstruction();

        world.calculateCurrentPopulation();
        world.calculatePopulationCap();

        user.HandleQuestProgress();
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse(0, 333);
    }
}
