using System.Text.Json.Serialization;

namespace CityVilleDotnet.Domain.GameEntities;

public class StorageDto
{
    [JsonPropertyName("goods")]
    public int Goods { get; set; }
    
    [JsonPropertyName("premium_goods")]
    public int PremiumGoods { get; set; }
}