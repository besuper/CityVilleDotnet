using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public (int CoinYield, int CashYield, List<int> SecureRands) HarvestObject(World world, WorldObject obj, long actionTime)
    {
        var itemName = obj.GetItemName();
        var gameItem = GameSettingsManager.Instance.GetItem(itemName) ?? throw new Exception($"Can't find game item for {itemName}");
        var contractName = obj.ContractName;

        var coinMultiplier = (obj.GetClassName().IsBusiness() ? Math.Max(obj.Visits ?? 0, 1) : 1) * GetMasteryBonusMultiplier(gameItem, "coinYield");
        var premiumGoodsMultiplier = GetMasteryBonusMultiplier(gameItem, "premium_goods");
        var workerBonus = gameItem.GetWorkerHarvestBonus();

        if (workerBonus?.Field == "premium_goods" && obj.Workers.Count > 0)
        {
            premiumGoodsMultiplier += obj.Workers.Count * workerBonus.PercentModifier / 100.0;
        }

        var (coinYield, cashYield) = obj.Harvest(actionTime);
        var secureRands = CollectDoobersRewards(itemName,
            coinMultiplier: coinMultiplier,
            premiumGoodsMultiplier: premiumGoodsMultiplier,
            goodsMultiplier: GetMasteryBonusMultiplier(gameItem, "goods"),
            xpMultiplier: GetMasteryBonusMultiplier(gameItem, "XP"),
            energyMultiplier: GetMasteryBonusMultiplier(gameItem, "energy"));

        if (obj.ClassName == BuildingClassType.HarvestableShip && contractName is not null)
        {
            world.CountCruiseUpgradeActions(obj, contractName);
        }

        return (coinYield, cashYield, secureRands);
    }

    public void TrackHarvest(World world, WorldObject obj, string itemName, string? contractName, IReadOnlyDictionary<string, int> experiments)
    {
        var className = obj.GetClassName();

        RollPreySpawns(world, GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}"), experiments);

        HandleQuestsProgress("harvestByClass", className: className.ToString());
        HandleQuestsProgress("harvestByKeyword", itemName: obj.ItemName);
        HandleQuestsProgress("harvestResidenceByName", itemName: obj.ItemName);
        HandleQuestsProgress("harvestResidenceByRegEx", itemName: obj.ItemName);
        HandleQuestsProgress("harvestItemByName", itemName: obj.ItemName);

        if (contractName is not null)
        {
            HandleQuestsProgress("harvestContractByName", itemName: contractName);
        }

        if (obj.ClassName == BuildingClassType.Plot || contractName is not null)
        {
            HandleQuestsProgress("harvestPlotByName", itemName: itemName);

            if (GameSettingsManager.Instance.GetItem(itemName)?.HasMasteries() == true)
            {
                IncrementMastery(itemName);
            }
        }

        if (obj.ClassName == BuildingClassType.Business)
        {
            HandleQuestsProgress("harvestBusinessByName", itemName: itemName);
            HandleQuestsProgress("harvestBusinessByClass", className: className.ToString());
            HandleQuestsProgress("harvestBusinessByKeyword", itemName: itemName);
        }
    }
}
