using CityVilleDotnet.Domain.Entities;
using System.Text.Json.Serialization;
using FluorineFx;

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
    
    [JsonPropertyName("lastExpansionTier")]
    public int LastExpansionTier { get; set; }

    [JsonPropertyName("world_id")]
    public required string WorldId { get; set; }
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
                PopulationSummary = new PopulationSummaryDto()
                {
                    Segments = new ASObject(new Dictionary<string, object>()
                    {
                        // TODO: Support multiple population types
                        {"citizen", new Dictionary<string, object>()
                        {
                            {"id", "citizen"},
                            {"minimum", 0},
                            {"yield", model.Population * 10},
                            {"maximum", model.PopulationCap * 10},
                            {"capacity", model.PopulationCap * 10},
                            {"potential", model.PotentialPopulation * 10}
                        }}
                    })
                }
            },
            Objects = model.Objects.Select(x => x.ToDto()).ToList(),
            LastExpansionTier = 0,
            WorldId = "world_main"
        };
    }
}