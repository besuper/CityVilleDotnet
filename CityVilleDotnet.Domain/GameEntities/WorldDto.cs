using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class WorldDto
{
    [JsonPropertyName("sizeX")]
    public int SizeX { get; set; } = 36;

    [JsonPropertyName("sizeY")]
    public int SizeY { get; set; } = 36;

    [JsonPropertyName("mapRects")]
    public List<MapRectDto> MapRects { get; set; }

    [JsonPropertyName("citySim")]
    public CitySimDto? CitySim { get; set; }

    [JsonPropertyName("objects")]
    public List<WorldObjectDto> Objects { get; set; }
}

public static class WorldDtoMapper
{
    public static WorldDto ToDto(this World model)
    {
        return new WorldDto()
        {
            SizeX = model.SizeX,
            SizeY = model.SizeY,
            MapRects = model.MapRects.Select(x => x.ToDto()).ToList(),
            CitySim = new CitySimDto()
            {
                Population = model.Population,
                PopulationCap = model.PopulationCap,
                PotentialPopulation = model.PotentialPopulation,
            },
            Objects = model.Objects.Select(x => x.ToDto()).ToList()
        };
    }
}