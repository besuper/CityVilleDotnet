using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.Entities;

public class Storage
{
    [JsonPropertyName("goods")]
    public int Goods { get; set; }
}
