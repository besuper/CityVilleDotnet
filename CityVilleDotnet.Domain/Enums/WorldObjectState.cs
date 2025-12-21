using System.ComponentModel;

namespace CityVilleDotnet.Domain.Enums;

public enum WorldObjectState
{
    [Description("open")]
    Open = 0,
    [Description("closed")]
    Closed = 1,
    [Description("static")]
    Static = 2,
    [Description("planted")]
    Planted = 3,
    [Description("openHarvestable")]
    OpenHarvestable = 4,
    [Description("closedHarvestable")]
    ClosedHarvestable = 5,
    [Description("grown")]
    Grown = 6,
    [Description("plowed")]
    Plowed = 7,
}