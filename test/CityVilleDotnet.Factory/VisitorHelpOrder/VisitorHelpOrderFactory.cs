using Bogus;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Factory.VisitorHelpOrder;

public static class VisitorHelpOrderFactory
{
    public static Domain.Entities.VisitorHelpOrder VisitorHelpOrder(
        this Faker faker,
        string? senderId = null,
        string? recipientId = null,
        int[]? helpTargets = null,
        TransmissionStatus transmissionStatus = TransmissionStatus.Received)
    {
        return new Domain.Entities.VisitorHelpOrder
        {
            SenderId = senderId ?? faker.Random.Int(1, 99999).ToString(),
            RecipientId = recipientId ?? faker.Random.Int(1, 99999).ToString(),
            HelpTargets = helpTargets ?? [faker.Random.Int(1, 9999)],
            Status = VisitorHelpStatus.Unclaimed,
            OrderState = OrderState.Pending,
            OrderType = OrderType.VisitorHelp,
            TransmissionStatus = transmissionStatus,
            TimeSent = faker.Random.Long(1, 2_000_000_000)
        };
    }
}
