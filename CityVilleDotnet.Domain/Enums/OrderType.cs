using System.ComponentModel;

namespace CityVilleDotnet.Domain.Enums;

public enum OrderType
{
    [Description("order_train")]
    Train = 0,
    [Description("order_lot")]
    Lot = 1,
    [Description("order_visitor_help")]
    VisitorHelp = 2
}