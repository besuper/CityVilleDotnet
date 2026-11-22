using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class PreyGroupDto
{
    [JsonPropertyName("preyId")] public required string PreyId { get; set; }

    [JsonPropertyName("preys")] public required List<int> Preys { get; set; }

    [JsonPropertyName("numPreysCaptured")] public int NumPreysCaptured { get; set; }

    [JsonPropertyName("typesPreysCaptured")] public required Dictionary<string, int> TypesPreysCaptured { get; set; }

    [JsonPropertyName("gateUnlockLevel")] public int GateUnlockLevel { get; set; }
}

public static class PreyGroupDtoMapper
{
    public static PreyGroupDto ToPreyGroupDto(this PreyGroup? group, string groupId)
    {
        return new PreyGroupDto
        {
            PreyId = groupId,
            Preys = group?.Preys.ToList() ?? [],
            NumPreysCaptured = group?.NumPreysCaptured ?? 0,
            TypesPreysCaptured = group?.Captures.ToDictionary(x => x.PreyId.ToString(), x => x.Count) ?? [],
            GateUnlockLevel = group?.GateUnlockLevel ?? 1
        };
    }
}
