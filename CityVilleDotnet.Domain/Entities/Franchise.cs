namespace CityVilleDotnet.Domain.Entities;

public class Franchise
{
    public int Id { get; set; }
    public string FranchiseType { get; set; }
    public string FranchiseName { get; set; }
    public long TimeLastCollected { get; set; }
    public List<FranchiseLocation> Locations { get; set; } = [];

    public Franchise(string franchiseType, string franchiseName)
    {
        FranchiseType = franchiseType;
        FranchiseName = franchiseName;
        TimeLastCollected = 0;
    }
    
    public void SetFranchiseName(string franchiseName)
    {
        FranchiseName = franchiseName;
    }

    public FranchiseLocation AddLocation(LotOrder order, int commodityReq)
    {
        var newLocation = new FranchiseLocation
        {
            Uid = order.RecipientId,
            ObjectId = $"{order.LotId}",
            FranchiseName = FranchiseName,
            StarRating = 1,
            TimeLastOperated = 0, // When receiver opened the business
            TimeLastSupplied = 0, // When sender supplied the business
            TimeLastCollected = 0, // When sender collected the business
            CommodityLeft = commodityReq,
            CommodityMax = commodityReq
        };

        Locations.Add(newLocation);

        return newLocation;
    }
}