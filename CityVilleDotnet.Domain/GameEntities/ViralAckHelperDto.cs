using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class ViralAckHelperDto
{
    [JsonPropertyName("zid")] public string Zid { get; set; } = string.Empty;

    [JsonPropertyName("itemName")] public string? ItemName { get; set; }

    [JsonPropertyName("ts")] public long Ts { get; set; }

    [JsonPropertyName("targetObjectName")] public string? TargetObjectName { get; set; }
}
