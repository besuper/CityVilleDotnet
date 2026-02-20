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
        var player = model.Player?.ToDto();

        player.Neighbors = model.Friends.Select(x => x.ToNeighborDto()).ToList();

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
                            { "commoditySummary", new ASObject { { "commodity", new ASObject() } } },
                        })
                    }
                })
            },
            Franchises = model.Player.Franchises.Select(x => x.ToDto()).ToList(),
            FeatureData = new ASObject(new Dictionary<string, object>()
            {
                { "cityAtNight", new ASObject() },
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
                { "goal", false },
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
            }), // Enable or disable some features for the user
        };
    }
}