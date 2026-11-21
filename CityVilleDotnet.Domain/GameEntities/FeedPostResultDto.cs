using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class FeedPostResultDto
{
    [JsonPropertyName("lastSuperFeedPostError")] public int LastSuperFeedPostError { get; set; }
}
