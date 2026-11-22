using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class FlagDto
{
    [JsonPropertyName("name")] public required string Name { get; set; }

    [JsonPropertyName("m_value")] public int Value { get; set; }

    [JsonPropertyName("lastModifiedGlobalEngineTime")] public long LastModifiedGlobalEngineTime { get; set; }
}

public static class FlagDtoMapper
{
    public static FlagDto ToDto(this Flag model)
    {
        return new FlagDto
        {
            Name = model.Name,
            Value = model.Value,
            LastModifiedGlobalEngineTime = model.LastModified.ToUnixTimeMilliseconds()
        };
    }
}
