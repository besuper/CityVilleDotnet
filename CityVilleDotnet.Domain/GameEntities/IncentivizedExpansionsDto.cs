using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class IncentivizedExpansionsDto
{
    // A null entry marks a completed expansion
    [JsonPropertyName("expansions")] public Dictionary<string, IncentivizedExpansionDto?> Expansions { get; set; } = [];

    [JsonPropertyName("cellToId")] public Dictionary<string, string> CellToId { get; set; } = [];

    [JsonPropertyName("parentExpansions")] public EmptyDto ParentExpansions { get; set; } = new();

    [JsonPropertyName("failureCount")] public Dictionary<string, int> FailureCount { get; set; } = [];
}

public class IncentivizedExpansionDto
{
    [JsonPropertyName("x")] public int X { get; set; }

    [JsonPropertyName("y")] public int Y { get; set; }

    [JsonPropertyName("start")] public long Start { get; set; }
}

public class WorldFeatureDataDto
{
    [JsonPropertyName("incentivizedExpansions")] public required IncentivizedExpansionsDto IncentivizedExpansions { get; set; }
}

public static class IncentivizedExpansionsDtoMapper
{
    public static IncentivizedExpansionsDto ToIncentivizedExpansionsDto(this World world)
    {
        var dto = new IncentivizedExpansionsDto();

        foreach (var expansion in world.IncentivizedExpansions)
        {
            if (expansion.IsCompleted)
            {
                dto.Expansions[expansion.ExpansionId] = null;
            }
            else if (expansion.IsActive())
            {
                dto.Expansions[expansion.ExpansionId] = new IncentivizedExpansionDto
                {
                    X = expansion.X!.Value,
                    Y = expansion.Y!.Value,
                    Start = expansion.StartTimestamp ?? 0
                };

                dto.CellToId[$"{expansion.X}_{expansion.Y}"] = expansion.ExpansionId;
            }

            if (expansion.FailureCount > 0)
            {
                dto.FailureCount[expansion.ExpansionId] = expansion.FailureCount;
            }
        }

        return dto;
    }
}
