using System.Text.Json.Serialization;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.GameEntities;

public class RewardForSpendFeatureDto
{
    [JsonPropertyName("version")] public int Version { get; set; }

    [JsonPropertyName("versionedData")] public RewardForSpendDataDto VersionedData { get; set; } = new();
}

public class RewardForSpendDataDto
{
    [JsonPropertyName("initialAmount")] public int InitialAmount { get; set; }

    [JsonPropertyName("progress")] public int Progress { get; set; }

    [JsonPropertyName("targetAmount")] public int TargetAmount { get; set; }
}

public class CityAtNightFeatureDto
{
    [JsonPropertyName("nightModeAvailable")] public bool NightModeAvailable { get; set; }

    [JsonPropertyName("earlyUnlocked")] public bool EarlyUnlocked { get; set; }

    [JsonPropertyName("cityLightCount")] public int CityLightCount { get; set; }

    [JsonPropertyName("active")] public bool Active { get; set; }

    [JsonPropertyName("numExpansionsPurchased")] public int NumExpansionsPurchased { get; set; }

    [JsonPropertyName("isPlazaBuild")] public bool IsPlazaBuild { get; set; }
}

public class RemodelFeatureDto
{
    [JsonPropertyName("enabled")] public bool Enabled { get; set; }
}

public class GoalFeatureDto
{
    [JsonPropertyName("mastery")] public required Dictionary<string, MasteryGoalDto> Mastery { get; set; }
}

public class MasteryGoalDto
{
    [JsonPropertyName("count")] public int Count { get; set; }

    [JsonPropertyName("level")] public int Level { get; set; }
}

public class WeatherFeatureDto
{
    [JsonPropertyName("disableWeatherEffect")] public bool DisableWeatherEffect { get; set; }

    [JsonPropertyName("supportedWeather")] public required Dictionary<string, int> SupportedWeather { get; set; }
}

public class LeaderboardsFeatureDto
{
    [JsonPropertyName("summaries")] public EmptyDto Summaries { get; set; } = new();
}

public class TaxisFeatureDto
{
    [JsonPropertyName("numInstaHarvests")] public int NumInstaHarvests { get; set; }

    [JsonPropertyName("lastResetTime")] public double LastResetTime { get; set; }

    [JsonPropertyName("maxInstaHarvests")] public int MaxInstaHarvests { get; set; }
}

public class CarsFeatureDto
{
    [JsonPropertyName("carsenabled")] public bool CarsEnabled { get; set; }

    [JsonPropertyName("craftenabled")] public bool CraftEnabled { get; set; }

    [JsonPropertyName("inventory")] public required Dictionary<string, int> Inventory { get; set; }
}

public class InventoryFeatureDto
{
    [JsonPropertyName("inventory")] public required Dictionary<string, int> Inventory { get; set; }
}

public class WorkersFeatureDto<TWorkers>
{
    [JsonPropertyName("workers")] public required TWorkers Workers { get; set; }
}

public class XpromoMetaDataDto
{
    [JsonPropertyName("level")] public int Level { get; set; }
}

public class SocialInventoryFeatureDto
{
    [JsonPropertyName("samObjectIds")] public required Dictionary<string, int> SamObjectIds { get; set; }
}

public static class FeatureDataDtoMapper
{
    // Enable or disable some features for the user
    public static Dictionary<string, object> ToFeatureDataDto(this Player model, List<World> summaryWorlds)
    {
        var featureData = new Dictionary<string, object>
        {
            ["rewardForSpend"] = new RewardForSpendFeatureDto(),
            ["cityAtNight"] = new CityAtNightFeatureDto
            {
                NightModeAvailable = model.NightModeAvailable,
                EarlyUnlocked = model.CityAtNightEarlyUnlocked,
                CityLightCount = model.CityLightCount,
                Active = model.NightModeActive,
                NumExpansionsPurchased = model.LightExpansionsPurchased,
                IsPlazaBuild = model.IsPlazaBuild
            },
            ["remodel"] = new RemodelFeatureDto { Enabled = model.GetWorld().HasRemodelHeadquarters() },
            ["gardens"] = new EmptyDto(),
            ["incentivizedExpansions"] = model.GetWorld().ToIncentivizedExpansionsDto(),
            ["helperClicks"] = new EmptyDto(),
            ["viralAck"] = model.ToViralAckDtos(),
            ["matchup"] = new EmptyDto(),
            ["trickOrTreat"] = new EmptyDto(),
            ["poll"] = new EmptyDto(),
            ["itemCounts"] = new EmptyDto(),
            ["rollCall"] = false,
            ["subway"] = new List<object>(),
            ["goal"] = new GoalFeatureDto
            {
                Mastery = model.Masteries.ToDictionary(x => x.ItemName, x => new MasteryGoalDto { Count = x.Count, Level = x.Level })
            },
            ["weather"] = new WeatherFeatureDto
            {
                DisableWeatherEffect = model.WeatherEffectDisabled,
                SupportedWeather = new() { ["0"] = 0, ["1"] = 1 }
            },
            ["leaderboards"] = new LeaderboardsFeatureDto(),
            ["taxis"] = new TaxisFeatureDto
            {
                NumInstaHarvests = model.TaxiNumInstaHarvests,
                LastResetTime = model.TaxiLastResetTime,
                MaxInstaHarvests = summaryWorlds
                    .SelectMany(w => w.Objects)
                    .Where(o => o.ClassName == BuildingClassType.TaxiDispatch)
                    .Select(o => o.GetTaxiMaxInstaHarvests())
                    .DefaultIfEmpty(3)
                    .Max()
            },
            ["cars"] = new CarsFeatureDto
            {
                CarsEnabled = model.CarsEnabled,
                CraftEnabled = model.CarCraftEnabled,
                Inventory = model.Cars.ToDictionary(x => x.Name, x => x.Amount)
            },
            ["crafting"] = new InventoryFeatureDto { Inventory = model.CraftingParts.ToDictionary(x => x.Name, x => x.Amount) },
            ["trains"] = new WorkersFeatureDto<Dictionary<string, WorkerManagerDto<TrainWorkerAttributesDto, EmptyDto>>> { Workers = model.GetWorld().ToTrainWorkersDto() },
            ["factories"] = new WorkersFeatureDto<Dictionary<string, WorkerManagerDto<FactoryWorkerAttributesDto, EmptyDto>>> { Workers = model.GetWorld().ToFactoryWorkersDto() },
            ["detectiveGameWorkerManager"] = new WorkersFeatureDto<EmptyDto> { Workers = new EmptyDto() },
            // FarmVille 2 (game id 118), max level to unlocks every checkFarmville2LevelX quest task
            ["xpromoMetaData"] = new Dictionary<string, XpromoMetaDataDto> { ["118"] = new() { Level = 20 } },
            ["socialInventory"] = new SocialInventoryFeatureDto { SamObjectIds = [] },
            ["globalTable"] = model.ToGlobalTableDto()
        };

        // The client reads featureData[groupId] for every prey group of the settings
        foreach (var setting in GameSettingsManager.Instance.GetPreyGroups())
        {
            var group = model.PreyGroups.FirstOrDefault(x => x.GroupId == setting.Id);

            featureData[setting.Id] = new WorkersFeatureDto<Dictionary<string, WorkerManagerDto<EmptyDto, HunterWorkerDataDto>>> { Workers = group.ToHunterWorkersDto() };
        }

        return featureData;
    }

    // GlobalTableOverrideManager::loadObject
    private static Dictionary<string, Dictionary<string, string>> ToGlobalTableDto(this Player model)
    {
        return model.GlobalTableOverrides
            .OrderBy(x => x.Id)
            .GroupBy(x => x.Keyword)
            .ToDictionary(keyword => keyword.Key, keyword => keyword
                .GroupBy(x => x.Table)
                .ToDictionary(table => table.Key, table => string.Join(",", table.Select(x => x.Source))));
    }
}
