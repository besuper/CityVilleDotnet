using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public partial class WorldObject
{
    // Airplane::getUnroundedCashCostToMakeReady
    private double AirplaneCostToMakeReady()
    {
        if (State != WorldObjectState.Planted || PlantTime is null) return 0;

        var settings = GameSettingsManager.Instance.GetSettings();
        var growTime = GameSettingsManager.Instance.GetItem(GetItemName())?.GrowTime ?? 0;
        var timeLeftMs = Math.Round(growTime * settings.InGameDaySeconds * 1000.0 * settings.GrowMultiplier) - (ServerUtils.GetCurrentTime() - PlantTime.Value);

        return Math.Max(timeLeftMs / 3600000.0 / (growTime * 24 * settings.GrowMultiplier * 2) + 0.5, 0.501);
    }

    // Airplane::allowInstantReady
    public bool AllowsInstantReady()
    {
        return ClassName != BuildingClassType.Airplane || GameSettingsManager.Instance.GetItem(ItemName)?.HasKeyword("noInstantReady") != true;
    }
}
