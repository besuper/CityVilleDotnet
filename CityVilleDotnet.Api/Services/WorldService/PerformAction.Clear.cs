using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformClear(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null when action type is clear");

        foreach (var item in building)
        {
            logger.LogInformation($"{item.Key} = {item.Value}");
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]) ?? throw new Exception($"Can't find building");

        var secureRands = user.CollectDoobersRewards(obj.ItemName);

        world.RemoveBuilding(obj);

        context.Set<WorldObject>().Remove(obj);

        user.HandleQuestProgress(itemName: obj.ClassName); // Wilderness
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse(333, new ASObject
        {
            ["secureRands"] = AmfConverter.Convert(secureRands)
        });
    }
}