using System.Text.Json.Serialization;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Domain.GameEntities;

public class OrderDto
{
    [JsonPropertyName("senderID")] public string SenderId { get; set; } = string.Empty;

    [JsonPropertyName("recipientID")] public string RecipientId { get; set; } = string.Empty;

    [JsonPropertyName("timeSent")] public long TimeSent { get; set; }

    [JsonPropertyName("lastTimeReminded")] public long LastTimeReminded { get; set; }

    [JsonPropertyName("orderType")] public string OrderType { get; set; } = string.Empty;

    [JsonPropertyName("orderState")] public string OrderState { get; set; } = string.Empty;

    [JsonPropertyName("transmissionStatus")] public string TransmissionStatus { get; set; } = string.Empty;
}

public class LotOrderDto : OrderDto
{
    [JsonPropertyName("lotId")] public int LotId { get; set; }

    [JsonPropertyName("resourceType")] public required string ResourceType { get; set; }

    [JsonPropertyName("orderResourceName")] public required string OrderResourceName { get; set; }

    [JsonPropertyName("constructionCount")] public int ConstructionCount { get; set; }

    [JsonPropertyName("offsetX")] public int? OffsetX { get; set; }

    [JsonPropertyName("offsetY")] public int? OffsetY { get; set; }
}

public class VisitorHelpOrderDto : OrderDto
{
    [JsonPropertyName("helpTargets")] public required int[] HelpTargets { get; set; }

    [JsonPropertyName("status")] public required string Status { get; set; }
}

public static class OrderDtoMapper
{
    // orderType -> transmissionStatus -> orderState -> other user id -> order
    public static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, OrderDto>>>> ToOrdersDto(this Player model)
    {
        var root = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, OrderDto>>>>();

        // TODO: Add TrainOrder
        foreach (var order in model.LotOrders.Where(x => x.OrderState == OrderState.Pending || (x.OrderState == OrderState.Accepted && x.TransmissionStatus == TransmissionStatus.Sent)))
        {
            AddOrder(root, order, new LotOrderDto
            {
                LotId = order.LotId,
                ResourceType = order.ResourceType,
                OrderResourceName = order.OrderResourceName,
                ConstructionCount = order.ConstructionCount,
                OffsetX = order.OffsetX,
                OffsetY = order.OffsetY
            });
        }

        foreach (var order in model.VisitorHelpOrders)
        {
            AddOrder(root, order, new VisitorHelpOrderDto
            {
                HelpTargets = order.HelpTargets,
                Status = order.Status.ToDescriptionString()
            });
        }

        return root;
    }

    private static void AddOrder(Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, OrderDto>>>> root, CommonOrder order, OrderDto dto)
    {
        dto.SenderId = order.SenderId;
        dto.RecipientId = order.RecipientId;
        dto.TimeSent = order.TimeSent;
        dto.LastTimeReminded = order.LastTimeReminded;
        dto.OrderType = order.OrderType.ToDescriptionString();
        dto.OrderState = order.OrderState.ToDescriptionString();
        dto.TransmissionStatus = order.TransmissionStatus.ToDescriptionString();

        var otherUserId = order.TransmissionStatus == TransmissionStatus.Received ? order.SenderId : order.RecipientId;

        var byTransmission = GetOrAdd(root, dto.OrderType);
        var byState = GetOrAdd(byTransmission, dto.TransmissionStatus);
        var byOtherUser = GetOrAdd(byState, dto.OrderState);

        byOtherUser[otherUserId] = dto;
    }

    private static T GetOrAdd<T>(Dictionary<string, T> dictionary, string key) where T : new()
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            value = new T();
            dictionary[key] = value;
        }

        return value;
    }
}
