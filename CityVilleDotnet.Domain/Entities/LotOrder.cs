namespace CityVilleDotnet.Domain.Entities;

public class LotOrder : CommonOrder
{
    public int LotId { get; set; }
    public required string ResourceType { get; set; }
    public required string OrderResourceName { get; set; }
    public int ConstructionCount { get; set; } = 0;
    public int? OffsetX { get; set; }
    public int? OffsetY { get; set; }

    // The client moves a received order to its first lot site when its lotId isn't one
    public void AcceptOnLot(int lotId)
    {
        LotId = lotId;
        Accept();
    }
}