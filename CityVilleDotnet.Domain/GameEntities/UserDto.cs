using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
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
            },
            Franchises = model.Player.Franchises.Select(x => x.ToDto()).ToList(),
            FeatureData = new ASObject(new Dictionary<string, object>()
            {
                { "cityAtNight", new ASObject() },
                { "remodel", new ASObject() { { "enabled", false } } },
                { "gardens", new ASObject() },
                { "incentivizedExpansions", new ASObject() },
                { "helperClicks", new ASObject() },
                { "viralAck", new ASObject() },
                { "matchup", new ASObject() },
                { "trickOrTreat", new ASObject() },
                { "poll", new ASObject() },
                { "itemCounts", new ASObject() },
                { "rollCall", false },
                { "goal", false },
                { "weather", new ASObject() },
                { "leaderboards", new ASObject() },
                
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