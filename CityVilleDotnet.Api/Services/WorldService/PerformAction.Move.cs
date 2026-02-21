using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformMove(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null when action type is move");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        if (!building.TryGetValue("id", out var id)) throw new Exception("Can't find id inside building element");

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");

        var obj = user.GetWorld().GetBuildingById(Convert.ToInt32(id)) ?? throw new Exception($"Can't find object with id {id}");

        obj.MoveTo(Convert.ToInt32(position["x"]), Convert.ToInt32(position["y"]), Convert.ToInt32(position["z"]), Convert.ToInt32(building["direction"]));

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}