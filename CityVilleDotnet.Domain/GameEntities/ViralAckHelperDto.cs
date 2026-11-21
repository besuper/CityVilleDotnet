using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class ViralAckHelperDto
{
    [JsonPropertyName("zid")] public string Zid { get; set; } = string.Empty;

    [JsonPropertyName("itemName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ItemName { get; set; }

    [JsonPropertyName("crewPos")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? CrewPos { get; set; }

    [JsonPropertyName("ts")] public long Ts { get; set; }

    [JsonPropertyName("targetObjectName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TargetObjectName { get; set; }
}
