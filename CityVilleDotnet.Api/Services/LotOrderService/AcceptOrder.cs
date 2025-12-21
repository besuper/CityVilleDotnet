using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.LotOrderService;

public class AcceptOrder(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var senderId = (string)@params[0];

        var receiveUser = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.LotOrders.Where(o => o.OrderState == OrderState.Pending && o.TransmissionStatus == TransmissionStatus.Received && o.SenderId == senderId))
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (receiveUser?.Player is null || receiveUser?.World is null) throw new Exception("Can't find player with UserId");

        if (receiveUser.Player.LotOrders.Count > 1) throw new Exception("Can't accept order, more than one order pending");

        var lotOrder = receiveUser.Player.LotOrders.FirstOrDefault(x => x.SenderId == senderId);

        if (lotOrder is null) throw new Exception("Can't find order with senderId");

        var senderPlayer = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.LotOrders.Where(o => o.OrderState == OrderState.Pending && o.TransmissionStatus == TransmissionStatus.Sent && o.LotId == lotOrder.LotId))
            .Include(x => x.Player)
            .ThenInclude(x => x!.Franchises.Where(f => f.FranchiseType == lotOrder.ResourceType && f.FranchiseName == lotOrder.OrderResourceName))
            .ThenInclude(x => x.Locations)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Where(x => x.Player!.Uid == senderId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (senderPlayer is null) throw new Exception("Can't find sender player");

        if (senderPlayer.LotOrders.Count > 1) throw new Exception("Can't accept order, more than one order pending");

        var senderLorOrder = senderPlayer.LotOrders.FirstOrDefault(x => x.LotId == lotOrder.LotId);

        if (senderLorOrder is null) throw new Exception("Can't find order with lotId");

        var senderFranchise = senderPlayer.Franchises.FirstOrDefault();

        if (senderFranchise is null) throw new Exception("Can't find sender franchise");

        var newBuilding = receiveUser.World.Objects.FirstOrDefault(x => x.WorldFlatId == lotOrder.LotId);

        if (newBuilding is null) throw new Exception("Can't find building with WorldFlatId");

        var gameItem = GameSettingsManager.Instance.GetItem(lotOrder.ResourceType);

        if (gameItem is null) throw new Exception($"Game item {lotOrder.ResourceType} not found");
        if (gameItem.HeadquartersName is null) throw new Exception($"Game item {lotOrder.ResourceType} does not have HeadquartersName defined");

        // FIXME: Accept or delete the request ?
        senderLorOrder.Accept();
        lotOrder.Accept();

        // FIXME: Building will be replaced, franchise owner will have a franchise in menu, but the franchise will not be correctly linked
        receiveUser.World.ReplaceBuildingFromLotOrder(lotOrder);

        var newLocation = senderFranchise.AddLocation(lotOrder);
        newBuilding.SetFranchiseLocation(newLocation);
        
        senderPlayer.AddItem(gameItem.HeadquartersName);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}