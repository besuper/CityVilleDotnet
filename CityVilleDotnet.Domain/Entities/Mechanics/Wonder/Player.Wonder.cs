using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    // TimerActionMechanic::processHarvest
    public void HarvestWonder(World world, WorldObject obj, GameItem item, DateTime actionTime, string? couponName, Dictionary<string, int> experiments)
    {
        var config = item.HarvestLoopConfig ?? throw new Exception($"No harvestLoopConfig for {item.Name}");

        if (IsHarvestLoopLocked(world, obj, config, actionTime))
        {
            throw new Exception($"Another {config.LockOnKeyword} object is already open");
        }

        var coupon = GetHarvestLoopCoupon(config, couponName);

        if (coupon is not null)
        {
            UseObjectCoupon(coupon, obj.WorldFlatId);
        }

        obj.StartHarvestLoop(config, actionTime, coupon);

        if (item.EnergyCost?.Harvest is not null)
        {
            RemoveEnergy(int.Parse(item.EnergyCost.Harvest));
        }

        CollectDoobersRewards(item.Name);
        RollPreySpawns(world, item, experiments);

        var className = obj.GetClassName().ToString();
        
        HandleQuestsProgress("harvestByClass", className: className);
        HandleQuestsProgress("harvestBusinessByClass", className: className);
        HandleQuestsProgress("harvestByKeyword", itemName: item.Name);
        HandleQuestsProgress("harvestMechanicByKeywords", itemName: item.Name);
        HandleQuestsProgress("harvestBusinessByKeyword", itemName: item.Name);
        HandleQuestsProgress("harvestItemByName", itemName: item.Name);
        HandleQuestsProgress("harvestBusinessByName", itemName: item.Name);
        HandleQuestsProgress("harvestResidenceByName", itemName: item.Name);
        HandleQuestsProgress("harvestResidenceByRegEx", itemName: item.Name);
    }

    // BaseMaintenanceMechanic::finishWithCash
    public void FinishWonderMaintenanceWithCash(WorldObject obj, GameItem item, DateTime now)
    {
        var config = item.HarvestLoopConfig ?? throw new Exception($"No harvestLoopConfig for {item.Name}");
        var mechanic = item.GetGameEventMechanic("maintenanceHarvestLoop") ?? throw new Exception($"No maintenanceHarvestLoop mechanic for {item.Name}");

        obj.FinishHarvestLoopMaintenance(config, now);
        
        RemoveCash(mechanic.FinishWithCash / mechanic.NumRequired * mechanic.NumRequired);
    }

    // TimerCouponMechanic::onAddToWorld, a coupon named after the item makes the new object ready
    public void UseAddToWorldHarvestLoopCoupon(GameItem item)
    {
        if (item.Mechanics?.GetMechanicByGameMode("onAddToWorld")?.GetMechanicItemByType("harvestLoop")?.ClassName != "TimerCouponMechanic")
        {
            return;
        }

        var coupon = Coupons.FirstOrDefault(x => x.Name == item.Name && x.WorldFlatId == null);

        if (coupon is not null)
        {
            Coupons.Remove(coupon);
        }
    }

    // TimerActionMechanic::hudTimerClicked
    public void BoostWonder(WorldObject obj, GameItem item, DateTime actionTime)
    {
        var config = item.HarvestLoopConfig ?? throw new Exception($"No harvestLoopConfig for {item.Name}");

        obj.BoostHarvestLoop(config, actionTime);
        
        RemoveCash(config.Cash);
        AddEnergy(config.EnergyGain);
    }

    // CouponSpendMechanic with couponData="peeps": selling the terminal spends the coupons of its waiting VIPs
    public void SpendWonderPeepCoupons(GameItem item)
    {
        var config = item.HarvestLoopConfig ?? throw new Exception($"No harvestLoopConfig for {item.Name}");
        var requiredCoupons = config.GetRequiredCoupons().ToList();

        Coupons.RemoveAll(x => x.WorldFlatId == null && requiredCoupons.Contains(x.Name));
    }

    // HarvestWonderPeepContractMechanic::getDoobers, the first harvest for a terminal rolls the FTUE table (the VIP coupon)
    // TODO: implement the fallback table (premium goods or inventory full) is not rolled
    public string HarvestWonderPeepContract(World world, WorldObject obj, GameItem gameItem, MechanicItem mechanic)
    {
        WorldObject? terminal = null;

        if (mechanic.TerminalKeyword is not null && obj.ParentMacroObjectId is not null)
        {
            terminal = world.Objects.FirstOrDefault(o => o != obj && o.ParentMacroObjectId == obj.ParentMacroObjectId && GameSettingsManager.Instance.GetItem(o.ItemName)?.HasKeyword(mechanic.TerminalKeyword) == true);
        }

        var useFtueTable = terminal is not null && mechanic.FtueTable is not null && !terminal.HasMechanicCounter(mechanic.FtueTable);

        if (useFtueTable)
        {
            terminal!.IncrementMechanicCounter(mechanic.FtueTable!);
        }

        return HarvestMechanicContract(obj, gameItem, mechanic, useFtueTable ? mechanic.FtueTable : null);
    }

    // TimerActionMechanic::isHarvestLocked
    private static bool IsHarvestLoopLocked(World world, WorldObject obj, HarvestLoopConfigItem config, DateTime now)
    {
        if (config.LockOnKeyword is null)
        {
            return false;
        }

        return world.Objects.Any(o => o != obj
                                      && GameSettingsManager.Instance.GetItem(o.ItemName) is { } item
                                      && item.HasKeyword(config.LockOnKeyword)
                                      && item.HarvestLoopConfig is { } otherConfig
                                      && o.IsHarvestLoopOpen(otherConfig, now));
    }

    // TimerCouponMechanic::getRequiredCoupon
    private static string? GetHarvestLoopCoupon(HarvestLoopConfigItem config, string? couponName)
    {
        var requiredCoupons = config.GetRequiredCoupons().ToList();

        if (requiredCoupons.Count == 1)
        {
            return requiredCoupons[0];
        }

        return couponName is not null && requiredCoupons.Contains(couponName) ? couponName : null;
    }

    // Player::useCoupon, a coupon associated to the object first, then a global one
    private void UseObjectCoupon(string name, int worldFlatId)
    {
        var coupon = Coupons.FirstOrDefault(x => x.Name == name && x.WorldFlatId == worldFlatId)
                     ?? Coupons.FirstOrDefault(x => x.Name == name && x.WorldFlatId == null)
                     ?? throw new Exception($"Missing coupon {name}");

        Coupons.Remove(coupon);
    }
}