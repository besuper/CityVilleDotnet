using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.VisitorService;

public sealed class DeclineHelp(CityVilleDbContext context) : AmfService<DeclineHelpRequest>
{
    public override async Task<ASObject> HandlePacket(DeclineHelpRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<Player>()
            .Include(x => x!.VisitorHelpOrders.Where(o =>
                o.SenderId == request.HelpOrder.SenderId &&
                o.RecipientId == request.HelpOrder.RecipientId &&
                o.Status == VisitorHelpStatus.Unclaimed))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null)
            throw new Exception("Player not found");

        var ordersToRemove = user.VisitorHelpOrders.ToList();

        foreach (var order in ordersToRemove)
        {
            user.VisitorHelpOrders.Remove(order);
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

public class DeclineHelpValidator : AbstractValidator<DeclineHelpRequest>
{
    public DeclineHelpValidator()
    {
        RuleFor(x => x.HelpOrder.SenderId).NotEmpty().MaximumLength(16);
        RuleFor(x => x.HelpOrder.RecipientId).NotEmpty().MaximumLength(16);
    }
}