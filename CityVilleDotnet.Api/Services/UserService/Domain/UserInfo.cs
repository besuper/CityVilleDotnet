using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.Domain;

public class UserInfo
{
    [JsonIgnore]
    public Guid Id { get; set; }

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
    public Player? Player { get; set; }

    [JsonPropertyName("world")]
    public World? World { get; set; }
}

