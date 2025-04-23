using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformClear(User user, object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var building = _params[1] as ASObject;

        if (building is null)
        {
            throw new Exception("Building can't be null when action type is place");
        }

        foreach (var item in building)
        {
            _logger.LogInformation($"{item.Key} = {item.Value}");
        }

        var position = building["position"] as ASObject;
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]);

        if (obj is null)
        {
            throw new Exception($"Can't find building");
        }

        var secureRands = user.CollectDoobersRewards(obj.ItemName);

        world.RemoveObject(obj);

        user.HandleQuestProgress(itemName: obj.ClassName); // Wilderness
        user.CheckCompletedQuests();

        await _context.SaveChangesAsync(cancellationToken);

        var response = new ASObject();
        response["secureRands"] = AmfConverter.Convert(secureRands);

        return new CityVilleResponse(333, response);
    }
}
