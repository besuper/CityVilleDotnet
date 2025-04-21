using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformFinish(User user, object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        var building = _params[1] as ASObject;

        if (building is null)
        {
            throw new Exception($"Building can't be null");
        }

        foreach (var item in building)
        {
            _logger.LogInformation($"{item.Key} = {item.Value}");
        }

        var position = building["position"] as ASObject;
        var itemId = (int)building["id"];
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]);

        if (obj is null)
        {
            throw new Exception($"Can't find building with ID {itemId}");
        }

        if (obj.Builds is null)
        {
            throw new Exception($"Can't find `builds`");
        }

        // Collect modifiers from construction stage
        user.CollectDoobersRewards(obj.ItemName);

        var newId = world.GetAvailableBuildingId();

        _logger.LogInformation($"Using new ID {newId}");

        obj.WorldFlatId = newId;

        obj.FinishConstruction();

        world.calculateCurrentPopulation();
        world.calculatePopulationCap();

        user.HandleQuestProgress();
        user.CheckCompletedQuests();

        await _context.SaveChangesAsync(cancellationToken);

        var rep = new ASObject();
        rep["id"] = newId;

        return new CityVilleResponse(333, rep);
    }
}
