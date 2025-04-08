using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.Entities;

public class CitySim
{
    [JsonPropertyName("population")]
    public int Population { get; set; }

    [JsonPropertyName("populationCap")]
    public int PopulationCap { get; set; }

    [JsonPropertyName("potentialPopulation")]
    public int PotentialPopulation { get; set; }
}