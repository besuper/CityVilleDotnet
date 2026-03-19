using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Enums;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class UserDto
{
    [JsonPropertyName("userInfo")] public required UserInfoDto UserInfo { get; set; }
    [JsonPropertyName("franchises")] public List<FranchiseDto> Franchises { get; set; } = new();
    [JsonPropertyName("featureData")] public required ASObject FeatureData { get; set; }
}

public static class UserDtoMapper
{
    public static UserDto ToDto(this User model)
    {
        var player = model.Player?.ToDto(model.World);

        player.Neighbors = model.Friends.Select(x => x.ToNeighborDto()).ToList();

        var activeQuests = model.Quests
            .Where(q => q.QuestType == QuestType.Active)
            .ToList();

        return new UserDto()
        {
            UserInfo = new UserInfoDto
            {
                CreationTimestamp = model.Player.CreationTimestamp,
                FirstDay = model.Player.FirstDay,
                IsNew = model.Player.IsNew,
                Player = player,
                World = model.World?.ToDto(),
                Username = model.Player.Username,
                WorldName = model.World.WorldName,
                // This fix null in setFinishedWorldFTUE after tutorial
                WorldSummary = new ASObject(new Dictionary<string, object>()
                {
                    {
                        "world_main", new ASObject(new Dictionary<string, object>()
                        {
                            { "world_id", "world_main" },
                            { "ftueCompleted", !model.Player.IsNew },
                            {
                                "items_by_name", model.World.Objects
                                    .Where(x => x.ClassName != BuildingClassType.ConstructionSite)
                                    .GroupBy(x => x.ItemName)
                                    .ToDictionary(g => g.Key, g => g.Count())
                            },
                            {
                                "construction_items", model.World.Objects
                                    .Where(x => x.ClassName == BuildingClassType.ConstructionSite && x.TargetBuildingName != null)
                                    .GroupBy(x => x.TargetBuildingName)
                                    .ToDictionary(g => g.Key, g => g.Count())
                            },
                            { "malls_items", new ASObject() }, // TODO: Implement containers
                            { "incentivized_expansion", new ASObject() }, // TODO: Implement specials expansions
                            { "numberOfExpansions", model.Player.ExpansionsPurchased },
                            { "number_of_business", model.World.Objects.Count(x => x.ClassName == BuildingClassType.Business) },
                            { "populationSummary", model.World.ToPopulationSummaryDto() },
                            {
                                "appraisalSummary", new ASObject() // TODO: Implement appraisal (not used in world_main)
                                {
                                    { "id", null },
                                    { "yield", 0 },
                                    { "capacity", 0 },
                                    { "potential", 0 },
                                }
                            },
                            { "commoditySummary", model.World.ToCommoditySummaryDto() },
                            {
                                "savedQuestSequence", activeQuests
                                    .Where(q => q.Location == QuestLocation.Sidebar)
                                    .OrderBy(q => q.Order)
                                    .Select(q => q.Name)
                                    .ToList()
                            },
                            {
                                "questManagerQuests", activeQuests
                                    .Where(q => q.Location == QuestLocation.Menu)
                                    .OrderBy(q => q.Order)
                                    .Select(q => q.Name)
                                    .ToList()
                            },
                            {
                                "hiddenQuests", activeQuests
                                    .Where(q => q.Location == QuestLocation.Hidden)
                                    .OrderBy(q => q.Order)
                                    .Select(q => q.Name)
                                    .ToList()
                            },
                        })
                    }
                })
            },
            Franchises = model.Player.Franchises.Select(x => x.ToDto()).ToList(),
            FeatureData = new ASObject(new Dictionary<string, object>()
            {
                {
                    "cityAtNight", new ASObject()
                    {
                        { "nightModeAvailable", true },
                        { "earlyUnlocked", true },
                        { "cityLightCount", 0 },
                        { "active", false },
                        { "numExpansionsPurchased", model.Player.ExpansionsPurchased },
                    }
                },
                { "remodel", new ASObject() { { "enabled", false } } },
                { "gardens", new ASObject() },
                {
                    "incentivizedExpansions", new ASObject()
                    {
                        { "expansions", new ASObject() },
                        { "cellToId", new ASObject() },
                        { "parentExpansions", new ASObject() },
                        { "failureCount", new ASObject() },
                    }
                },
                { "helperClicks", new ASObject() },
                { "viralAck", new ASObject() },
                { "matchup", new ASObject() },
                { "trickOrTreat", new ASObject() },
                { "poll", new ASObject() },
                { "itemCounts", new ASObject() },
                { "rollCall", false },
                {
                    "goal", new ASObject
                    {
                        {
                            "mastery", model.Player.Masteries.ToDictionary(x => x.ItemName, x => new ASObject
                            {
                                { "count", x.Count },
                                { "level", x.Level },
                            })
                        }
                    }
                },
                { "weather", new ASObject() },
                {
                    "leaderboards", new ASObject
                    {
                        { "summaries", new ASObject() }
                    }
                },

                // Prey groups
                // FIXME: load them from gameSettings
                { "copsNBandits", new ASObject { { "workers", new ASObject() } } },
                { "animalRescue", new ASObject { { "workers", new ASObject() } } },
                { "downtownpolice", new ASObject { { "workers", new ASObject() } } },
                { "fishing", new ASObject { { "workers", new ASObject() } } },
                { "area51", new ASObject { { "workers", new ASObject() } } },
                { "trains", new ASObject { { "workers", new ASObject() } } },
                { "factories", new ASObject { { "workers", new ASObject() } } }, // TODO: Implement hiring workers
                { "detectiveGameWorkerManager", new ASObject { { "workers", new ASObject() } } }
            }), // Enable or disable some features for the user
        };
    }
}