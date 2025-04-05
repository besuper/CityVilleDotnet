using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.Domain;

public class Commodities
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonPropertyName("storage")]
    public Storage? Storage { get; set; }
}
