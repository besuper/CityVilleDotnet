using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.Domain;

public class World
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonPropertyName("sizeX")]
    public int SizeX { get; set; } = 36;

    [JsonPropertyName("sizeY")]
    public int SizeY { get; set; } = 36;

    [JsonPropertyName("mapRects")]
    public List<MapRect> MapRects { get; set; }

    [JsonPropertyName("citySim")]
    public CitySim? CitySim { get; set; }

    [JsonPropertyName("objects")]
    public List<WorldObject> Objects { get; set; }

    internal void AddBuilding(WorldObject obj)
    {
        Objects.Add(obj);
    }
}
