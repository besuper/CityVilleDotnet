using System.ComponentModel;

namespace CityVilleDotnet.Domain.Enums;

public enum WorldType
{
    [Description("world_main")] Main = 0,
    [Description("world_downtown")] Downtown = 1,
    [Description("world_lakefront")] Lakefront = 2,
}