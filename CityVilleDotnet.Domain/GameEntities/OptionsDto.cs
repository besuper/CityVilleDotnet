using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class OptionsDto
{
    [JsonPropertyName("sfxDisabled")]
    public bool SfxDisabled { get; set; } = false;

    [JsonPropertyName("musicDisabled")]
    public bool MusicDisabled { get; set; } = false;
}