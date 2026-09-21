using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class OnRemoveFromInventory(CityVilleDbContext context) : AmfService<OnRemoveFromInventoryRequest>
{
    public override async Task<ASObject> HandlePacket(OnRemoveFromInventoryRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.InventoryItems.Where(i => i.Name == request.ItemName))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var owned = player.CountInventoryItem(request.ItemName);

        if (owned > 0)
        {
            player.RemoveItem(request.ItemName, Math.Min(request.Amount, owned));

            await context.SaveChangesAsync(cancellationToken);
        }

        return GatewayService.CreateEmptyResponse();
    }
}

public class OnRemoveFromInventoryRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public int Amount { get; set; }
}

public class OnRemoveFromInventoryValidator : AbstractValidator<OnRemoveFromInventoryRequest>
{
    public OnRemoveFromInventoryValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
