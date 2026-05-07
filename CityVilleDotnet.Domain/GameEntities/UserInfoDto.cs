using System.Text.Json.Serialization;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class UserInfoDto
{
    [JsonPropertyName("worldName")]
    public string WorldName { get; set; } = "";

    [JsonPropertyName("is_new")]
    public bool IsNew { get; set; }

    [JsonPropertyName("firstDay")]
    public bool FirstDay { get; set; }

    [JsonPropertyName("creationTimestamp")]
    public int CreationTimestamp { get; set; } = 0;

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("player")]
    public required PlayerDto Player { get; set; }

    [JsonPropertyName("world")]
    public required WorldDto World { get; set; }

    [JsonPropertyName("world_summary")] public ASObject WorldSummary { get; set; } = [];
    [JsonPropertyName("CompletedQuests")] public List<string> CompletedQuests { get; set; } = [];
    [JsonPropertyName("lastPlayedWorldId")] public required string LastPlayedWorldId { get; set; }
}