using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class MapRectDto
{
    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}

public static class MapRectDtoMapper
{
    public static MapRectDto ToDto(this MapRect model)
    {
        return new MapRectDto()
        {
            X = model.X,
            Y = model.Y,
            Width = model.Width,
            Height = model.Height
        };
    }
}