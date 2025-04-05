using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.Domain;

public class Options
{
    [JsonPropertyName("sfxDisabled")]
    public bool SfxDisabled { get; set; } = false;

    [JsonPropertyName("musicDisabled")]
    public bool MusicDisabled { get; set; } = false;
}
