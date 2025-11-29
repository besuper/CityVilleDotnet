using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Domain.GameEntities;

public class FranchiseLocationDto
{
    [JsonPropertyName("star_rating")] public int StarRating { get; set; }

    [JsonPropertyName("commodity_left")] public int CommodityLeft { get; set; }

    [JsonPropertyName("commodity_max")] public int CommodityMax { get; set; }

    [JsonPropertyName("customers_served")] public int CustomersServed { get; set; }

    [JsonPropertyName("money_collected")] public int MoneyCollected { get; set; }

    [JsonPropertyName("obj_id")] public required string ObjectId { get; set; }

    [JsonPropertyName("time_last_collected")]
    public int TimeLastCollected { get; set; }

    [JsonPropertyName("time_last_operated")]
    public int TimeLastOperated { get; set; }

    [JsonPropertyName("time_last_supplied")]
    public int TimeLastSupplied { get; set; }
}

public static class FranchiseLocationDtoMapper
{
    public static FranchiseLocationDto ToDto(this FranchiseLocation model)
    {
        return new FranchiseLocationDto
        {
            StarRating = model.StarRating,
            CommodityLeft = model.CommodityLeft,
            CommodityMax = model.CommodityMax,
            CustomersServed = model.CustomersServed,
            MoneyCollected = model.MoneyCollected,
            ObjectId = model.ObjectId,
            TimeLastCollected = model.TimeLastCollected,
            TimeLastOperated = model.TimeLastOperated,
            TimeLastSupplied = model.TimeLastSupplied
        };
    }
}