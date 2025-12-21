namespace CityVilleDotnet.Domain.Entities;

public class LotOrder : CommonOrder
{
    public int LotId { get; set; }
    public required string ResourceType { get; set; }
    public required string OrderResourceName { get; set; }
    public int ConstructionCount { get; set; } = 0;
    public int? OffsetX { get; set; }
    public int? OffsetY { get; set; }
}