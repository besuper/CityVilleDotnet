namespace CityVilleDotnet.Domain.Entities;

public class Franchise
{
    public int Id { get; set; }
    public string FranchiseType { get; set; }
    public string FranchiseName { get; set; }
    public int TimeLastCollected { get; set; }
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
}