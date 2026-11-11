using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public partial class WorldObject
{
    // ThemeParkSupplyMechanic::getRequiredCommodities
    public int GetThemeParkSupplyCost()
    {
        return Slots.Sum(x => GetThemeParkSlotCommodityRequired(x.SlotIndex));
    }

    // ThemeParkSupplyMechanic::generateHarvestState, the filled slots are frozen until the harvest
    public void OpenThemeParkSupply(int playerLevel)
    {
        if (IsStorageSupplied()) throw new Exception("Building is already supplied");

        SupplyResources.AddRange(Slots.OrderBy(x => x.SlotIndex).Select(x => new WorldObjectSupplyResource(x.SlotIndex, x.ItemName)));
        Visits = 0;
        State = WorldObjectState.Open;
        CountBusinessUpgradeAction(GameSettingsManager.Instance.GetItem(ItemName), playerLevel);
    }

    // BaseSupplyMechanic::getCustomerCapacity
    public int GetThemeParkCustomersReq()
    {
        return SupplyResources.Sum(x => GetThemeParkSlotCommodityRequired(x.Position));
    }

    private int GetThemeParkSlotCommodityRequired(int slotIndex)
    {
        return GameSettingsManager.Instance.GetItem(ItemName)?.ThemeParkStorage?.Slots.ElementAtOrDefault(slotIndex)?.CommodityRequired ?? 0;
    }
}
