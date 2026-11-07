using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    // StorageSupplyMechanic::updatePlayerCommodities + awardUpgradeActions
    public void SupplyStorage(IReadOnlyCollection<WorldObject> storedObjects)
    {
        var commodities = WorldObject.GetStorageSupplyCommodities(storedObjects);

        if (commodities.Count == 0) throw new DomainException(GameErrorType.InvalidState);
        if (commodities.Any(x => GetGoodsByType(x.Type) < x.Amount)) throw new DomainException(GameErrorType.NotEnoughMoney);

        foreach (var (type, amount) in commodities)
        {
            RemoveGoodByType(type, amount);
        }

        foreach (var stored in storedObjects)
        {
            stored.CountBusinessUpgradeAction(GameSettingsManager.Instance.GetItem(stored.ItemName), Level);
        }
    }

    // MallHarvestMechanic::preHarvest + StorageStateHarvestMechanic::processHarvest
    public void CollectStorageSupplyRewards(GameItem containerItem, IReadOnlyCollection<string> resourceNames)
    {
        var resources = resourceNames
            .Select(x => GameSettingsManager.Instance.GetItem(x))
            .OfType<GameItem>()
            .ToList();

        var harvestMechanic = containerItem.GetGameEventMechanic("harvestState");
        var mysteryBusinessReq = harvestMechanic?.MysteryBusinessReq ?? 0;

        if (mysteryBusinessReq > 0)
        {
            var modifierGroup = resources.Count >= mysteryBusinessReq ? containerItem.Name : harvestMechanic!.PreviousState;

            if (modifierGroup is not null)
            {
                CollectDoobersRewards(containerItem.Name, modifierGroup);
            }
        }

        // CustomerStorageStateHarvestMechanic::applyPayoutMultipliers
        var bonusPercent = 100 + GameSettingsManager.Instance.GetCommodityBonus(containerItem.GetDefaultCommodityName());

        foreach (var resource in resources)
        {
            var customers = resource.CustomerCapacity > 0 ? resource.CustomerCapacity.Value : resource.CommodityRequired ?? 0;

            CollectDoobersRewards(resource.Name, coinMultiplier: customers * bonusPercent / 100.0);
        }
    }
}
