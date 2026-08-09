using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public class TrainOrder
{
    public int Id { get; set; }
    public string ItemName { get; private set; }
    public TrainOperationType Operation { get; private set; }
    public string CommodityName { get; private set; }
    public long TimeSent { get; private set; }
    public List<TrainOrderWorker> Workers { get; private set; } = [];

    private TrainOrder()
    {
    }

    public TrainOrder(string itemName, TrainOperationType operation, string commodityName, long timeSent)
    {
        ItemName = itemName;
        Operation = operation;
        CommodityName = commodityName;
        TimeSent = timeSent;
    }

    public int GetSpeedUpCost()
    {
        return GetItem()?.TrainSpeedUpCost ?? 0;
    }

    public bool HasArrived(long currentTimeSeconds)
    {
        return TimeSent + GetTripTime() <= currentTimeSeconds;
    }

    public void SpeedUp(long currentTimeSeconds)
    {
        TimeSent = currentTimeSeconds - GetTripTime();
    }

    public int GetStopCashCost()
    {
        return GetItem()?.Workers?.CashCost ?? 0;
    }

    public int GetMaxStops()
    {
        return GetItem()?.GetMaxWorkers() ?? 0;
    }

    public int CountPurchasedStops()
    {
        return Workers.Count(x => x.IsPurchased());
    }

    public void AddPurchasedStop()
    {
        if (Workers.Count >= GetMaxStops())
            throw new Exception($"No stop left on train {ItemName}");

        Workers.Add(new TrainOrderWorker(-(CountPurchasedStops() + 1)));
    }

    // TrainWorkers::recalculateRewards
    public int GetPayout(bool hasGoldenTrainStatue, bool hasPayoutBonusUnlocked)
    {
        var bonus = 1.0;

        if (Workers.Count > 0 && hasPayoutBonusUnlocked)
            bonus = GameSettingsManager.Instance.GetSettings().TrainBonusMult;

        if (hasGoldenTrainStatue)
            bonus += 0.1;

        return (int)Math.Ceiling(GameSettingsManager.Instance.GetTieredValue(GetItem()?.TrainPayout?.Table, Workers.Count) * bonus);
    }

    private int GetTripTime()
    {
        return GetItem()?.TrainTripTime ?? 0;
    }

    private GameItem? GetItem()
    {
        return GameSettingsManager.Instance.GetItem(ItemName);
    }
}
