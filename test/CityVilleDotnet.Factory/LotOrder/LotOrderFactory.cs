using Bogus;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Factory.LotOrder;

public static class LotOrderFactory
{
    public static Domain.Entities.LotOrder LotOrder(
        this Faker faker,
        string? senderId = null,
        string? recipientId = null,
        int? lotId = null,
        TransmissionStatus transmissionStatus = TransmissionStatus.Received,
        string? resourceType = null)
    {
        return new Domain.Entities.LotOrder
        {
            SenderId = senderId ?? faker.Random.Int(1, 99999).ToString(),
            RecipientId = recipientId ?? faker.Random.Int(1, 99999).ToString(),
            LotId = lotId ?? faker.Random.Int(1, 9999),
            ResourceType = resourceType ?? faker.Random.String2(64),
            OrderResourceName = faker.Random.String2(64),
            OrderState = OrderState.Pending,
            OrderType = OrderType.Lot,
            TransmissionStatus = transmissionStatus,
            TimeSent = faker.Random.Long(1, 2_000_000_000)
        };
    }
}
