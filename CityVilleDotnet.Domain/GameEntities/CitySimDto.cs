using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class CitySimDto
{
    [JsonPropertyName("populationSummary")]
    public required PopulationSummaryDto PopulationSummary { get; set; }
}