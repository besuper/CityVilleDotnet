using CityVilleDotnet.Common.Settings.GameSettings;

namespace CityVilleDotnet.Domain.Entities;

public partial class WorldObject
{ 
    private static readonly TimeSpan HarvestLoopTolerance = TimeSpan.FromMinutes(1);

    public DateTime? HarvestLoopStartTime { get; private set; }
    public string? AppliedCoupon { get; private set; }

    // Wonder::isOpen, TimedHarvestableResource::isOpen
    public bool IsHarvestLoopOpen(HarvestLoopConfigItem config, DateTime now)
    {
        return HarvestLoopStartTime?.AddSeconds(config.GetTimerDuration(AppliedCoupon)) > now;
    }

    // TimerActionMechanic::isTimeReady + processHarvest, TimerCouponMechanic::preProgressHarvest store the applied coupon
    public void StartHarvestLoop(HarvestLoopConfigItem config, DateTime actionTime, string? coupon)
    {
        var readyTime = HarvestLoopStartTime?.AddSeconds(config.GetTimerDuration(AppliedCoupon) + config.SleepDuration);

        if (coupon is null && readyTime > actionTime + HarvestLoopTolerance)
        {
            throw new Exception($"Harvest loop of {ItemName} is not ready");
        }

        HarvestLoopStartTime = actionTime;

        if (coupon is not null)
        {
            AppliedCoupon = coupon;
        }
    }

    // TimerActionMechanic::boostTime
    public void BoostHarvestLoop(HarvestLoopConfigItem config, DateTime actionTime)
    {
        if (!IsHarvestLoopOpen(config, actionTime - HarvestLoopTolerance))
        {
            throw new Exception($"Harvest loop of {ItemName} is not open");
        }

        HarvestLoopStartTime = HarvestLoopStartTime!.Value.AddSeconds(config.BoostSeconds);
    }

    // BaseMaintenanceMechanic::onFinishMechanic
    public void FinishHarvestLoopMaintenance(HarvestLoopConfigItem config, DateTime now)
    {
        if (HarvestLoopStartTime is null || IsHarvestLoopOpen(config, now))
        {
            throw new Exception($"Harvest loop of {ItemName} is not sleeping");
        }

        HarvestLoopStartTime = null;
    }
}