using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public const string LightExpansionName = "expand_12_12_light_level";

    public int CityLightCount { get; private set; }
    public int LightLevel { get; private set; } = 1;
    public bool CityAtNightEarlyUnlocked { get; private set; }
    public bool NightModeAvailable { get; private set; }
    public int LightExpansionsPurchased { get; private set; }

    // this work only with CITY_AT_NIGHT_GAME_MODE = 1 and cv_CityatNights_Main = 3 !!
    private bool IsCityAtNightCore()
    {
        return Level >= 20;
    }

    // cityatnightUnlocked with CITY_AT_NIGHT_GAME_MODE = 1 only
    private bool IsCityAtNightUnlocked()
    {
        return IsCityAtNightCore() && CityAtNightEarlyUnlocked;
    }

    private bool? IsCityAtNightModifierValid(string validate)
    {
        return validate switch
        {
            "cityatnightModeActive" or "cityatnightDoober1" => IsCityAtNightUnlocked() && NightModeActive,
            "cityatnightInitialRewards" => IsCityAtNightCore(),
            _ when validate.StartsWith("cityatnight") => false,
            _ => null
        };
    }

    // From StartUpDialogHelper::earlyUnlockCityAtNight
    public void UnlockCityAtNight(int cashCost)
    {
        if (!IsCityAtNightCore() || CityAtNightEarlyUnlocked)
        {
            throw new DomainException(GameErrorType.InvalidState, "City at Night can't be unlocked");
        }

        RemoveCash(cashCost);
        CityAtNightEarlyUnlocked = true;

        HandleQuestsProgress("unlockCityAtNight");
    }

    // From CityAtNightMechanic::executeOverrideForGameEvent (onAddToWorld)
    public void OpenCityAtNight(string finishedItemName)
    {
        NightModeAvailable = true;

        CollectDoobersRewards(finishedItemName, modifiersName: "initial");
    }

    // From CityAtNightData::incrementCityLightCount
    private void AddCityLights(int amount)
    {
        CityLightCount += amount;

        HandleQuestsProgress("countLights", amount: amount);
    }

    // From CityAtNightData::recalcLightLevel
    public int GetComputedLightLevel()
    {
        return Math.Max(GameSettingsManager.Instance.GetLightLevels().Count(x => x.RequiredLights <= CityLightCount), 1);
    }

    // From CityAtNightManager::handleLightLevelUp
    public void LightLevelUp()
    {
        if (LightLevel >= GetComputedLightLevel())
        {
            throw new DomainException(GameErrorType.InvalidState, $"Light level {LightLevel} can't level up with {CityLightCount} lights");
        }

        LightLevel++;

        var reward = GameSettingsManager.Instance.GetLightLevels().FirstOrDefault(x => x.Num == LightLevel)?.Rewards;

        if (reward is null)
        {
            return;
        }

        AddCoins(reward.Coins);
        AddGoods(reward.Goods);

        if (reward.Energy > 0)
        {
            AddEnergy(reward.Energy);
        }

        if (reward.Item is not null)
        {
            AddItem(reward.Item, reward.ItemCount);
        }
    }

    // From LightLevelRequirement::getNumNeeded
    public bool HasPassedLightLevelGate()
    {
        return HasLicense(LightExpansionName) || GetComputedLightLevel() >= GetNextLightExpansion().LightLevel;
    }

    // From ExpansionDialog::buyLightLevel, cost from LightLevelRequirement::getNextExpansionUnlockCost
    public void AcquireLightExpansionLicense()
    {
        if (HasLicense(LightExpansionName))
        {
            return;
        }

        RemoveCash(HasPassedLightLevelGate() ? 0 : Math.Max(GetNextLightExpansion().Cost, 0));
        AddLicense(LightExpansionName);
    }

    // From GMExpand, light level expansion only increments the city night counter and expires its license
    public LicenseItem? PurchaseLightExpansion()
    {
        LightExpansionsPurchased++;

        return DeleteLicense(LightExpansionName);
    }

    // From ExpansionManager::nextExpansionLevelConfigLightLevel
    private ExpansionSetting GetNextLightExpansion()
    {
        var expansions = GameSettingsManager.Instance.GetExpansions("lightlevel");

        return expansions[Math.Min(LightExpansionsPurchased + 1, expansions.Count) - 1];
    }
}
