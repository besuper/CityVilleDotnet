using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.LotOrderService;

public sealed class DenyOrder(CityVilleDbContext context) : AmfService<DenyOrderRequest>
{
    public override async Task<ASObject> HandlePacket(DenyOrderRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var order = request.Order;

        var player = await context.Set<Player>()
            .Include(x => x.LotOrders.Where(o =>
                o.OrderState == OrderState.Pending
                && o.TransmissionStatus == TransmissionStatus.Received
                && o.SenderId == order.SenderId
                && o.LotId == order.LotId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Can't find player");

        var receivedOrder = player.LotOrders.FirstOrDefault();

        if (receivedOrder is null)
        {
            throw new DomainException(GameErrorType.InvalidState, $"Can't find pending received order from sender {order.SenderId} for lot {order.LotId}");
        }

        var senderPlayer = await context.Set<Player>()
            .Include(x => x.LotOrders.Where(o =>
                o.OrderState == OrderState.Pending
                && o.TransmissionStatus == TransmissionStatus.Sent
                && o.RecipientId == receivedOrder.RecipientId
                && o.LotId == order.LotId))
            .FirstOrDefaultAsync(x => x.Snuid == int.Parse(order.SenderId), cancellationToken);

        context.Set<LotOrder>().Remove(receivedOrder);

        var sentOrder = senderPlayer?.LotOrders.FirstOrDefault();

        if (sentOrder is not null)
        {
            context.Set<LotOrder>().Remove(sentOrder);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class DenyOrderRequest
{
    [AmfParam(0)] public PlaceOrderParams Order { get; set; } = new();
}

public class DenyOrderRequestValidator : AbstractValidator<DenyOrderRequest>
{
    public DenyOrderRequestValidator()
    {
        RuleFor(x => x.Order.SenderId).NotEmpty().Matches("^[0-9]{1,9}$");
    }
}