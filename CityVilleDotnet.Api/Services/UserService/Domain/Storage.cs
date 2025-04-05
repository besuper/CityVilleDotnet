using System.Text.Json.Serialization;

namespace CityVilleDotnet.Api.Services.UserService.Domain;

public class Storage
{
    [JsonPropertyName("goods")]
    public int Goods { get; set; }
}
