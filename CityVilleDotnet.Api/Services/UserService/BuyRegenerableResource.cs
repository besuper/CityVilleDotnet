using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyRegenerableResource(CityVilleDbContext context) : AmfService<BuyRegenerableResourceRequest>
{
    public override async Task<ASObject> HandlePacket(BuyRegenerableResourceRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.BuyGas(request.ItemName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class BuyRegenerableResourceRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}

public class BuyRegenerableResourceRequestValidator : AbstractValidator<BuyRegenerableResourceRequest>
{
    public BuyRegenerableResourceRequestValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}
