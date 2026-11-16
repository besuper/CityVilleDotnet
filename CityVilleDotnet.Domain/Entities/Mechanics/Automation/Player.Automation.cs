using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public int Gas { get; private set; }
    public long GasLastCheck { get; private set; }

    // Player::regenerateRegenerableResource
    public (int Gas, long LastCheck) GetCurrentGas(long nowSeconds)
    {
        var settings = GameSettingsManager.Instance.GetRegenerableResource("gas");
        var gas = Gas;
        var lastCheck = GasLastCheck;

        while (gas < settings.RegenCap && nowSeconds - lastCheck >= settings.RegenInterval)
        {
            gas += settings.RegenAmount;
            lastCheck += settings.RegenInterval;
        }

        return (gas, lastCheck);
    }

    public void RegenerateGas(long nowSeconds)
    {
        (Gas, GasLastCheck) = GetCurrentGas(nowSeconds);
    }

    public void AddGas(int amount)
    {
        RegenerateGas(ServerUtils.GetCurrentTimeSeconds());
        Gas += amount;
    }

    // TBuyRegenerableResource
    public void BuyGas(string itemName)
    {
        var item = GameSettingsManager.Instance.GetItem(itemName) ?? throw new DomainException(GameErrorType.InvalidData);
        var gas = item.RegenerableRewards.Where(x => x.Name == "gas").Sum(x => x.Amount);

        if (gas <= 0)
        {
            throw new DomainException(GameErrorType.InvalidData);
        }

        if (item.Cost > 0)
        {
            RemoveCoins(item.Cost.Value);
        }
        else if (item.Cash > 0)
        {
            RemoveCash(item.Cash.Value);
        }

        AddGas(gas);
    }

    // ActionAutomationManager::automate
    public void PayAutomation(IReadOnlyCollection<WorldObject> objects, long nowSeconds)
    {
        var settings = GameSettingsManager.Instance.GetSettings();

        if (Level < settings.AutomationRequiredLevel || objects.Count == 0)
        {
            throw new DomainException(GameErrorType.InvalidState);
        }

        var energyCost = objects.Sum(o => int.TryParse(GameSettingsManager.Instance.GetItem(o.ItemName)?.EnergyCost?.Harvest, out var cost) ? cost : 0);

        if (objects.Count >= settings.AutomationNumEntitiesBeforeSavingsKickIn)
        {
            energyCost -= (int)Math.Round(energyCost * settings.AutomationCostSavingsRatio, MidpointRounding.AwayFromZero);
        }

        RemoveEnergy(energyCost);
        UseGas(objects.Count, nowSeconds);
    }

    // Player::updateRegenerableResource
    private void UseGas(int amount, long nowSeconds)
    {
        RegenerateGas(nowSeconds);

        if (Gas < amount)
        {
            throw new DomainException(GameErrorType.ForceReload);
        }

        if (Gas >= GameSettingsManager.Instance.GetRegenerableResource("gas").RegenCap)
        {
            GasLastCheck = nowSeconds;
        }

        Gas -= amount;
    }
}
