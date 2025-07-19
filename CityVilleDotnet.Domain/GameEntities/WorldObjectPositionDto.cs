using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class WorldObjectPositionDto
{
    [JsonPropertyName("x")] public int X { get; set; }

    [JsonPropertyName("y")] public int Y { get; set; }

    [JsonPropertyName("z")] public int? Z { get; set; }
}