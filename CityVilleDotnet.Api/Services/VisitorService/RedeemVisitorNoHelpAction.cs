using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.VisitorService;

public sealed class RedeemVisitorNoHelpAction(CityVilleDbContext context) : AmfService<RedeemVisitorNoHelpActionRequest>
{
    public override async Task<ASObject> HandlePacket(RedeemVisitorNoHelpActionRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.VisitorHelpOrders.Where(o => o.SenderId == request.VisitorId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        foreach (var order in player.VisitorHelpOrders.Where(o => o.SenderId == request.VisitorId).ToList())
        {
            player.VisitorHelpOrders.Remove(order);
            context.Remove(order);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class RedeemVisitorNoHelpActionRequest
{
    [AmfParam(0)] public string VisitorId { get; set; } = string.Empty;
}

public class RedeemVisitorNoHelpActionValidator : AbstractValidator<RedeemVisitorNoHelpActionRequest>
{
    public RedeemVisitorNoHelpActionValidator()
    {
        RuleFor(x => x.VisitorId).NotEmpty().MaximumLength(16);
    }
}
