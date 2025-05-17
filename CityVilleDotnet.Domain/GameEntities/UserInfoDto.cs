using System.Text.Json.Serialization;

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
    public string Username { get; set; }

    [JsonPropertyName("player")]
    public PlayerDto? Player { get; set; }

    [JsonPropertyName("world")]
    public WorldDto? World { get; set; }
}