using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.LotOrderService;

// TODO: Rework this transaction
public class PlaceOrder(CityVilleDbContext context) : AmfService<PlaceOrderRequest>
{
    public override async Task<ASObject> HandlePacket(PlaceOrderRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var order = request.Order;

        // TODO: Check senderId
        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Include(x => x.Player)
            .ThenInclude(x => x!.LotOrders)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        // TODO: Check friendship
        var receiverPlayer = await context.Set<Player>()
            .Include(x => x.LotOrders)
            .FirstOrDefaultAsync(x => x.Uid == order.RecipientId, cancellationToken);

        if (receiverPlayer is null) throw new Exception("Can't find player with recipientId");

        //TODO Remove gold or cash

        var lotOrder = new LotOrder
        {
            LotId = order.LotId,
            SenderId = order.SenderId,
            RecipientId = order.RecipientId,
            OffsetX = order.OffsetX,
            OffsetY = order.OffsetY,
            ConstructionCount = order.ConstructionCount,
            OrderResourceName = order.OrderResourceName,
            ResourceType = order.ResourceType,
            OrderState = OrderState.Pending,
            OrderType = OrderType.Lot,
            TransmissionStatus = TransmissionStatus.Sent
        };

        var receivedLotOrder = new LotOrder
        {
            LotId = order.LotId,
            SenderId = order.SenderId,
            RecipientId = order.RecipientId,
            OffsetX = order.OffsetX,
            OffsetY = order.OffsetY,
            ConstructionCount = order.ConstructionCount,
            OrderResourceName = order.OrderResourceName,
            ResourceType = order.ResourceType,
            OrderState = OrderState.Pending,
            OrderType = OrderType.Lot,
            TransmissionStatus = TransmissionStatus.Received
        };

        player.AddLotOrder(lotOrder);
        receiverPlayer.AddLotOrder(receivedLotOrder);

        await context.SaveChangesAsync(cancellationToken);

        // TODO: Implement return
        return GatewayService.CreateEmptyResponse();
    }
}

public class PlaceOrderParams
{
    [AmfParam("senderID")] public string SenderId { get; set; } = string.Empty;
    [AmfParam("recipientID")] public string RecipientId { get; set; } = string.Empty;
    [AmfParam("offsetX")] public int? OffsetX { get; set; }
    [AmfParam("offsetY")] public int? OffsetY { get; set; }
    [AmfParam("constructionCount")] public int ConstructionCount { get; set; }
    [AmfParam("lotId")] public int LotId { get; set; }
    [AmfParam("orderResourceName")] public string OrderResourceName { get; set; } = string.Empty;
    [AmfParam("resourceType")] public string ResourceType { get; set; } = string.Empty;
}

public class PlaceOrderRequest
{
    [AmfParam(0)] public PlaceOrderParams Order { get; set; } = new();
}