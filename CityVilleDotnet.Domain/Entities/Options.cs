using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.Entities;

public class Options
{
    [JsonPropertyName("sfxDisabled")]
    public bool SfxDisabled { get; set; } = false;

    [JsonPropertyName("musicDisabled")]
    public bool MusicDisabled { get; set; } = false;
}
