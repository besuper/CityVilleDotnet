using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.Entities;

public class VisitorHelpOrder : CommonOrder
{
    public VisitorHelpStatus Status { get; set; }
    public required int[] HelpTargets { get; set; }
}