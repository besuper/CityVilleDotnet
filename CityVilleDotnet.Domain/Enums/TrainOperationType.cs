using System.ComponentModel;

namespace CityVilleDotnet.Domain.Enums;

public enum TrainOperationType
{
    [Description("buy")]
    Buy = 0,
    [Description("sell")]
    Sell = 1,
}
