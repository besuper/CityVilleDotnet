using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.LotOrderService;

public class PlaceOrder(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var objParams = (ASObject)@params[0];
        
        var senderId = (string)objParams["senderID"];
        var recipientId = (string)objParams["recipientID"];
        var offsetX = (int?)objParams["offsetX"];
        var offsetY = (int?)objParams["offsetY"];
        var constructionCount = (int)objParams["constructionCount"];
        var lotId = (int)objParams["lotId"];
        var orderResourceName = (string)objParams["orderResourceName"];
        var resourceType = (string)objParams["resourceType"];
        
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
            .FirstOrDefaultAsync(x => x.Uid == recipientId, cancellationToken);
        
        if(receiverPlayer is null) throw new Exception("Can't find player with recipientId");

        //TODO Remove gold or cash
        
        var lotOrder = new LotOrder
        {
            Id = Guid.NewGuid(),
            LotId = lotId,
            SenderId = senderId,
            RecipientId = recipientId,
            OffsetX = offsetX,
            OffsetY = offsetY,
            ConstructionCount = constructionCount,
            OrderResourceName = orderResourceName,
            ResourceType = resourceType,
            OrderState = OrderState.Pending,
            OrderType = OrderType.Lot,
            TransmissionStatus = TransmissionStatus.Sent
        };
        
        var receivedLotOrder = new LotOrder
        {
            Id = Guid.NewGuid(),
            LotId = lotId,
            SenderId = senderId,
            RecipientId = recipientId,
            OffsetX = offsetX,
            OffsetY = offsetY,
            ConstructionCount = constructionCount,
            OrderResourceName = orderResourceName,
            ResourceType = resourceType,
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