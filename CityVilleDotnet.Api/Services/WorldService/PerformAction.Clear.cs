using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformClear(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null when action type is clear");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(Convert.ToInt32(position["x"]), Convert.ToInt32(position["y"]), Convert.ToInt32(position["z"])) ?? throw new Exception($"Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.EnergyCost?.Clear is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Clear);

            if (!user.Player!.RemoveEnergy(energyCost))
            {
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
            }
        }

        var secureRands = user.Player!.CollectDoobersRewards(obj.ItemName);

        world.RemoveBuilding(obj);

        context.Set<WorldObject>().Remove(obj);

        user.HandleQuestsProgress("clearByClass", className: obj.ClassName.ToString()); // Wilderness
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["secureRands"] = AmfConverter.Convert(secureRands)
        });
    }
}