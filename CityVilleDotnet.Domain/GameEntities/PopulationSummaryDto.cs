using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class PopulationSummaryDto
{
    [JsonPropertyName("segments")]
    public required Dictionary<string, SimSummaryDto> Segments { get; set; }
}

public static class PopulationSummaryDtoMapper
{
    public static PopulationSummaryDto ToPopulationSummaryDto(this World model)
    {
        return new PopulationSummaryDto()
        {
            // TODO: Support multiple population types
            Segments = new Dictionary<string, SimSummaryDto>
            {
                ["citizen"] = new()
                {
                    Id = "citizen",
                    Minimum = model.PopulationMin,
                    Yield = model.Population,
                    Maximum = model.PopulationMax,
                    Capacity = model.PopulationCap,
                    Potential = model.PotentialPopulation
                }
            }
        };
    }
}
