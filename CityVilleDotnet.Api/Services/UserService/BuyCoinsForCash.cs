using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyCoinsForCash(CityVilleDbContext context) : AmfService<BuyCoinsForCashRequest>
{
    public override async Task<ASObject> HandlePacket(BuyCoinsForCashRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.BuyCoinsForCash(request.Cash);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyCoinsForCashRequest
{
    [AmfParam(0)] public int Cash { get; set; }
}

public class BuyCoinsForCashRequestValidator : AbstractValidator<BuyCoinsForCashRequest>
{
    public BuyCoinsForCashRequestValidator()
    {
        RuleFor(x => x.Cash).GreaterThan(0).LessThanOrEqualTo(100_000);
    }
}
