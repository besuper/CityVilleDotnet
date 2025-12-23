using System.ComponentModel;

namespace CityVilleDotnet.Domain.Enums;

public enum VisitorHelpStatus
{
    [Description("unclaimed")] Unclaimed = 0,
    [Description("rejected")] Rejected = 1,
    [Description("claimed")] Claimed = 2
}