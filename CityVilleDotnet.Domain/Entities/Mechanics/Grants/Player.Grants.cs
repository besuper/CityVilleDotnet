using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Common.Utils;

namespace CityVilleDotnet.Domain.Entities;

public partial class Player
{
    public void GrantEligibleItems(Dictionary<string, int> experiments, List<string> ownedItemNames)
    {
        var now = ServerUtils.TimeProvider.GetUtcNow();
        var owned = ownedItemNames.Concat(InventoryItems.Where(x => x.Amount > 0).Select(x => x.Name)).ToHashSet();

        var grants = GameSettingsManager.Instance.GetGrantItems()
            .Where(x => Flags.All(f => f.Name != GetGrantFlagName(x.Key)))
            .DistinctBy(x => (x.Key, x.NamesList, x.Name))
            .Where(x => x.Validator is null || GameSettingsManager.Instance.GetValidator(x.Validator)?.IsValid(f => IsGrantFuncValid(f, experiments, now)) == true)
            .ToList();

        foreach (var name in grants.SelectMany(x => x.GetNames()))
        {
            if (!GameSettingsManager.Instance.GetOrderedUpgradeChainByRoot(name).Any(owned.Contains))
            {
                AddItem(name);
            }
        }

        foreach (var key in grants.Select(x => x.Key).Distinct())
        {
            SetFlag(GetGrantFlagName(key), 1);
        }
    }

    private static string GetGrantFlagName(string key)
    {
        return $"grant_{key}";
    }

    private bool IsGrantFuncValid(ValidateFunc func, IReadOnlyDictionary<string, int> experiments, DateTimeOffset now)
    {
        var variant = func.Experiment is null ? 0 : experiments.GetValueOrDefault(func.Experiment);

        // From ExperimentValidationUtil, SunsetValidationUtil and ScriptingConditionsUtil
        return func.Name switch
        {
            "playerRequiredLevel" => Level >= func.Level,
            "isVariantGreaterThan" => variant > func.Variant,
            "isVariantLessThan" => (variant != 0 || func.DiscardZero) && variant < func.Variant,
            "isInSimpleExperiment" => variant == 1,
            "isNotSunset" => GameSettingsManager.Instance.GetSunset(func.ForTheme ?? string.Empty) is not { } sunset || !sunset.IsSunset(now),
            _ => false
        };
    }
}
