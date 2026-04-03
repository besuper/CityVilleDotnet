using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.LotOrderService;

public class AcceptOrder(CityVilleDbContext context) : AmfService<AcceptOrderRequest>
{
    public override async Task<ASObject> HandlePacket(AcceptOrderRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var receiveUser = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x!.LotOrders.Where(o =>
                o.OrderState == OrderState.Pending
                && o.TransmissionStatus == TransmissionStatus.Received
                && o.SenderId == request.SenderId.ToString() // FIXME: Change all IDs to int
                && o.LotId == request.LotId))
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (receiveUser is null)
            throw new Exception("Can't find player");

        var lotOrder = receiveUser.LotOrders.FirstOrDefault();

        if (lotOrder is null)
            throw new Exception($"Can't find pending received order from sender {request.SenderId} for lot {request.LotId}");

        var senderPlayer = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x!.LotOrders.Where(o =>
                o.OrderState == OrderState.Pending
                && o.TransmissionStatus == TransmissionStatus.Sent
                && o.LotId == request.LotId))
            .Include(x => x!.Franchises.Where(f =>
                f.FranchiseType == lotOrder.ResourceType
                && f.FranchiseName == lotOrder.OrderResourceName))
            .ThenInclude(x => x.Locations)
            .Include(x => x!.InventoryItems)
            .FirstOrDefaultAsync(x => x.Snuid == request.SenderId, cancellationToken);

        if (senderPlayer is null)
            throw new Exception("Can't find sender player");

        var senderLotOrder = senderPlayer.LotOrders.FirstOrDefault();

        if (senderLotOrder is null)
            throw new Exception($"Can't find sender's pending sent order for lot {request.LotId}");

        var senderFranchise = senderPlayer.Franchises.FirstOrDefault();

        if (senderFranchise is null)
            throw new Exception("Can't find sender franchise");

        var newBuilding = receiveUser.GetWorld().Objects.FirstOrDefault(x => x.WorldFlatId == lotOrder.LotId);

        if (newBuilding is null)
            throw new Exception($"Can't find building with WorldFlatId {lotOrder.LotId}");

        var gameItem = GameSettingsManager.Instance.GetItem(lotOrder.ResourceType);

        if (gameItem is null)
            throw new Exception($"Game item {lotOrder.ResourceType} not found");
        if (gameItem.HeadquartersName is null)
            throw new Exception($"Game item {lotOrder.ResourceType} does not have HeadquartersName defined");

        senderLotOrder.Accept();
        lotOrder.Accept();

        receiveUser.GetWorld().ReplaceBuildingFromLotOrder(lotOrder);

        var newLocation = senderFranchise.AddLocation(lotOrder, gameItem.CommodityRequired ?? 1);
        newBuilding.SetFranchiseLocation(newLocation, request.SenderId.ToString());

        senderPlayer.AddItem(gameItem.HeadquartersName);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class AcceptOrderRequest
{
    [AmfParam(0)] public int SenderId { get; set; }
    [AmfParam(1)] public int LotId { get; set; }
}