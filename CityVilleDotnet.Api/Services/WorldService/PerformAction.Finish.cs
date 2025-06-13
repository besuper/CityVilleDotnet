using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformFinish(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception($"Building can't be null");

        foreach (var item in building)
        {
            logger.LogInformation($"{item.Key} = {item.Value}");
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var itemId = (int)building["id"];
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]) ?? throw new Exception($"Can't find building with ID {itemId}");

        if (obj.Builds is null)
        {
            throw new Exception($"Can't find `builds`");
        }

        // Collect modifiers from construction stage
        user.CollectDoobersRewards(obj.ItemName);

        var newId = world.GetAvailableBuildingId();

        logger.LogInformation($"Using new ID {newId}");

        obj.WorldFlatId = newId;

        obj.FinishConstruction();

        world.calculateCurrentPopulation();
        world.calculatePopulationCap();

        user.HandleQuestProgress();
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse(333, new ASObject
        {
            ["id"] = newId
        });
    }
}
