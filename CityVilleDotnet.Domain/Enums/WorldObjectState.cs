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
    [Description("base")]
    Base = 8,
    [Description("straight_NE_SW")]
    StraightNeSw = 9,
    [Description("straight_SW_NE")]
    StraightSwNe = 10,
    [Description("straight_NW_SE")]
    StraightNwSe = 11,
    [Description("straight_SE_NW")]
    StraightSeNw = 12,
    [Description("curved_NW_NE")]
    CurvedNwNe = 13,
    [Description("curved_NE_NW")]
    CurvedNeNw = 14,
    [Description("curved_NE_SE")]
    CurvedNeSe = 15,
    [Description("curved_SE_NE")]
    CurvedSeNe = 16,
    [Description("curved_SE_SW")]
    CurvedSeSw = 17,
    [Description("curved_SW_SE")]
    CurvedSwSe = 18,
    [Description("curved_SW_NW")]
    CurvedSwNw = 19,
    [Description("curved_NW_SW")]
    CurvedNwSw = 20,
}