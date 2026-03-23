using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.VisitorService;

public sealed class DeclineHelp(CityVilleDbContext context) : AmfService<DeclineHelpRequest>
{
    public override async Task<ASObject> HandlePacket(DeclineHelpRequest request, Guid userId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.HelpOrder.SenderId) || string.IsNullOrEmpty(request.HelpOrder.RecipientId))
            throw new Exception("SenderId or RecipientId can't be null");

        var user = await context.Set<User>()
            .Include(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders.Where(o =>
                o.SenderId == request.HelpOrder.SenderId &&
                o.RecipientId == request.HelpOrder.RecipientId &&
                o.Status == VisitorHelpStatus.Unclaimed))
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null)
            throw new Exception($"Can't find user with userId {userId}");

        var ordersToRemove = user.Player.VisitorHelpOrders.ToList();

        foreach (var order in ordersToRemove)
        {
            user.Player.VisitorHelpOrders.Remove(order);
            context.Remove(order);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class DeclineHelpRequest
{
    [AmfParam(0)] public HelpOrder HelpOrder { get; set; } = new();
}

public class HelpOrder
{
    [AmfParam("senderID")] public string SenderId { get; set; } = string.Empty;
    [AmfParam("recipientID")] public string RecipientId { get; set; } = string.Empty;
}