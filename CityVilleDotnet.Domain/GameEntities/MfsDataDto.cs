using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class MfsDataDto
{
    [JsonPropertyName("nonAppFriends")] public List<SocialNetworkUserDto> NonAppFriends { get; set; } = [];

    [JsonPropertyName("appFriends")] public List<SocialNetworkUserDto> AppFriends { get; set; } = [];
}
