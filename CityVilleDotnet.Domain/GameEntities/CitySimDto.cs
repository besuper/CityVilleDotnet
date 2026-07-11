using System.Text.Json.Serialization;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class CitySimDto
{
    [JsonPropertyName("populationSummary")]
    public required PopulationSummaryDto PopulationSummary { get; set; }

    [JsonPropertyName("appraisalSummary")]
    public ASObject? AppraisalSummary { get; set; }
}