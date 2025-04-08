using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.Entities;

public class MapRect
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}
