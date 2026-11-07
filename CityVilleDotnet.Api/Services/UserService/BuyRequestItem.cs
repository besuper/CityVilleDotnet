using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyRequestItem(CityVilleDbContext context) : AmfService<BuyRequestItemRequest>
{
    public override async Task<ASObject> HandlePacket(BuyRequestItemRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.RequestInventoryItems.Where(i => i.InventoryName == request.InventoryName))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.BuyRequestItem(request.ItemName, request.InventoryName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class BuyRequestItemRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public string InventoryName { get; set; } = string.Empty;
}

public class BuyRequestItemRequestValidator : AbstractValidator<BuyRequestItemRequest>
{
    public BuyRequestItemRequestValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.InventoryName).NotEmpty().MaximumLength(64);
    }
}
