using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed partial class PerformAction
{
    private async Task<CityVilleResponse> PerformHarvest(User user, object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var building = @params[1] as ASObject ?? throw new Exception("Building can't be null");

        foreach (var item in building)
        {
            logger.LogDebug("{ItemKey} = {ItemValue}", item.Key, item.Value);
        }

        var position = building["position"] as ASObject ?? throw new Exception("Can't find position inside building element");
        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(Convert.ToInt32(position["x"]), Convert.ToInt32(position["y"]), Convert.ToInt32(position["z"])) ?? throw new Exception("Can't find building");

        var itemName = obj.ClassName == BuildingClassType.Plot ? obj.ContractName : obj.ItemName;

        if (itemName is null)
            throw new Exception("Item name is null, can't harvest");

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {itemName}");

        if (gameItem.EnergyCost?.Harvest is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Harvest);

            if (!user.Player!.RemoveEnergy(energyCost))
            {
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
            }
        }

        var coinYield = obj.Harvest();
        var secureRands = user.Player!.CollectDoobersRewards(obj.ContractName ?? obj.ItemName);

        logger.LogDebug("Secure rands {Join}", string.Join(",", secureRands.ToArray()));
        logger.LogDebug("Secure rands {SecureRandsCount}", secureRands.Count);

        user.HandleQuestsProgress("harvestByClass", className: obj.ClassName.ToString());

        if (obj.ClassName == BuildingClassType.Plot)
        {
            user.HandleQuestsProgress("harvestPlotByName", itemName: obj.ItemName);
        }

        if (obj.ClassName == BuildingClassType.Business)
        {
            user.HandleQuestsProgress("harvestBusinessByName", itemName: obj.ItemName);
            user.HandleQuestsProgress("harvestBusinessByClass", className: obj.ClassName.ToString());
        }

        if (obj.ClassName == BuildingClassType.Residence)
        {
            user.HandleQuestsProgress("harvestResidenceByName", itemName: obj.ItemName);
        }

        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["retCoinYield"] = coinYield,
            //response["doobers"] = AmfConverter.Convert(new List<int>());
            ["secureRands"] = AmfConverter.Convert(secureRands)
        }).MetaData(CreateQuestComponentResponse(user));
    }
}