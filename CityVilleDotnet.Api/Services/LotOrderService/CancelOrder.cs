using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.LotOrderService;

public sealed class CancelOrder(CityVilleDbContext context) : AmfService<CancelOrderRequest>
{
    public override async Task<ASObject> HandlePacket(CancelOrderRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var order = request.Order;

        var player = await context.Set<Player>()
            .Include(x => x.LotOrders.Where(o =>
                o.OrderState == OrderState.Pending
                && o.TransmissionStatus == TransmissionStatus.Sent
                && o.RecipientId == order.RecipientId
                && o.LotId == order.LotId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Can't find player");

        var sentOrder = player.LotOrders.FirstOrDefault();

        if (sentOrder is null)
        {
            throw new DomainException(GameErrorType.InvalidState, $"Can't find pending sent order to recipient {order.RecipientId} for lot {order.LotId}");
        }

        var gameItem = GameSettingsManager.Instance.GetItem(sentOrder.ResourceType) ?? throw new Exception($"Game item {sentOrder.ResourceType} not found");

        var recipientPlayer = await context.Set<Player>()
            .Include(x => x.LotOrders.Where(o =>
                o.OrderState == OrderState.Pending
                && o.TransmissionStatus == TransmissionStatus.Received
                && o.SenderId == sentOrder.SenderId
                && o.LotId == order.LotId))
            .FirstOrDefaultAsync(x => x.Snuid == int.Parse(order.RecipientId), cancellationToken);

        context.Set<LotOrder>().Remove(sentOrder);

        var receivedOrder = recipientPlayer?.LotOrders.FirstOrDefault();

        if (receivedOrder is not null)
        {
            context.Set<LotOrder>().Remove(receivedOrder);
        }

        player.RefundLotOrder(gameItem);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class CancelOrderRequest
{
    [AmfParam(0)] public PlaceOrderParams Order { get; set; } = new();
}

public class CancelOrderRequestValidator : AbstractValidator<CancelOrderRequest>
{
    public CancelOrderRequestValidator()
    {
        RuleFor(x => x.Order.RecipientId).NotEmpty().Matches("^[0-9]{1,9}$");
    }
}
