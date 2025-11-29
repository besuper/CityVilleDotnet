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
    public required string ObjectId { get; set; }
    public int TimeLastCollected { get; set; }
    public int TimeLastOperated { get; set; }
    public int TimeLastSupplied { get; set; }
}