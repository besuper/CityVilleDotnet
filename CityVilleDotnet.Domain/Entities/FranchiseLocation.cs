namespace CityVilleDotnet.Domain.Entities;

public class FranchiseLocation
{
    public int Id { get; set; }
    public required string Uid { get; set; } // The targeted friend Uid
    public int StarRating { get; set; }
    public int CommodityLeft { get; set; }
    public int CommodityMax { get; set; }
    public int CustomersServed { get; set; }
    public int MoneyCollected { get; set; }
    public required string ObjectId { get; set; } // LotId that is WorldFlatId
    public long TimeLastCollected { get; set; }
    public long TimeLastOperated { get; set; }
    public long TimeLastSupplied { get; set; }
}