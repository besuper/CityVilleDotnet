using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class NeighborDto
{
    [JsonPropertyName("uid")] public string Uid { get; set; }
    [JsonPropertyName("fake")] public int? Fake { get; set; }
    [JsonPropertyName("empty")] public bool Empty { get; set; } = false;

    [JsonPropertyName("level")] public int Level { get; set; }
    [JsonPropertyName("gold")] public int Gold { get; set; }
    [JsonPropertyName("xp")] public int Xp { get; set; }
    [JsonPropertyName("cityname")] public string CityName { get; set; }
    [JsonPropertyName("socialLevel")] public int SocialLevel { get; set; }

    [JsonPropertyName("firstTimeVisit")] public bool FirstTimeVisit { get; set; }
    [JsonPropertyName("rollCall")] public bool RollCall { get; set; }
    [JsonPropertyName("collect")] public bool Collect { get; set; }
    [JsonPropertyName("nonSNNeighbor")] public bool NonSNNeighbor { get; set; }
    [JsonPropertyName("isSubscriber")] public bool IsSubscriber { get; set; }

    [JsonPropertyName("energyLeft")] public int EnergyLeft { get; set; }

    [JsonPropertyName("lastLoginTimestamp")]
    public int LastLoginTimestamp { get; set; }

    [JsonPropertyName("helpRequests")] public int HelpRequests { get; set; }

    [JsonPropertyName("playerClassType")] public int PlayerClassType { get; set; }

    [JsonPropertyName("zid")] public int Zid { get; set; }
    [JsonPropertyName("snuid")] public int Snuid { get; set; }
    [JsonPropertyName("snid")] public int Snid { get; set; }
}

public static class NeighborDtoMapper
{
    public static NeighborDto ToNeighborDto(this Friend model)
    {
        return new NeighborDto()
        {
            Uid = model.FriendPlayer.Snuid.ToString(),
            Zid = model.FriendPlayer.Snuid,
            Snuid = model.FriendPlayer.Snuid,
            Snid = model.FriendPlayer.Snuid,
            Level = model.FriendPlayer.Level,

            Gold = model.FriendPlayer.Gold,
            Xp = model.FriendPlayer.Xp,
            SocialLevel = model.FriendPlayer.SocialLevel,
            CityName = model.FriendPlayer.GetWorld().WorldName ?? "Unknown city",

            FirstTimeVisit = false,
            RollCall = false,
            Collect = false,
            NonSNNeighbor = false,
            IsSubscriber = false,

            EnergyLeft = model.EnergyLeft,
            LastLoginTimestamp = 0,
            HelpRequests = 0,
            PlayerClassType = -1
        };
    }
}