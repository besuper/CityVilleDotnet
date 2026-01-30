using System.Text.Json.Serialization;
using FluorineFx;

namespace CityVilleDotnet.Domain.GameEntities;

public class PopulationSummaryDto
{
    [JsonPropertyName("segments")]
    public required ASObject Segments { get; set; }
}