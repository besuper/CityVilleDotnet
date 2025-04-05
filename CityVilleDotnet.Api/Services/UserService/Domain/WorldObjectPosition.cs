using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.Domain;

public class WorldObjectPosition
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("z")]
    public int? Z { get; set; }
}
