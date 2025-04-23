using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
	private async Task PerformBuild(User user, object[] _params, Guid userId, CancellationToken cancellationToken)
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

        var obj = user.GetWorld().GetBuildingByCoord((int)position["x"], (int)position["y"], (int)position["z"]);

        if (obj is null)
        {
            throw new Exception($"Can't find building");
        }

        if (obj.Builds is null)
        {
            throw new Exception($"Can't find `builds`");
        }

        obj.AddConstructionStage();

        await _context.SaveChangesAsync(cancellationToken);
    }
}