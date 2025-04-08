using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.Entities;

public class Commodities
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonPropertyName("storage")]
    public Storage? Storage { get; set; }
}
