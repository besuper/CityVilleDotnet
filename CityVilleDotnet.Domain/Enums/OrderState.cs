using System.ComponentModel;

namespace CityVilleDotnet.Domain.Enums;

public enum OrderState
{
    [Description("pending")]
    Pending = 0,
    [Description("accepted")]
    Accepted = 1,
    [Description("denied")]
    Denied = 2
}