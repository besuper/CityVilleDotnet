namespace CityVilleDotnet.Domain.Entities;

public class FranchiseLocation
{
    private static readonly int[] StarThresholds = [0, 5, 15, 30, 50];

    public int Id { get; set; }
    public required string Uid { get; set; }
    public int StarRating { get; set; }
    public int CommodityLeft { get; set; }
    public int CommodityMax { get; set; }
    public int CustomersServed { get; set; }
    public int MoneyCollected { get; set; }
    public required string ObjectId { get; set; }
    public string FranchiseName { get; set; } = string.Empty;
    public long TimeLastCollected { get; set; }
    public long TimeLastOperated { get; set; }
    public long TimeLastSupplied { get; set; }

    public bool TryLevelUpStar()
    {
        if (StarRating >= 5) return false;

        var newRating = StarRating;

        for (var i = StarRating; i < StarThresholds.Length; i++)
        {
            if (CustomersServed >= StarThresholds[i])
                newRating = i + 1;
        }

        if (newRating == StarRating) return false;

        StarRating = newRating;
        return true;
    }
}