using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyConsumable(CityVilleDbContext context, ILogger<BuyConsumable> logger) : AmfService<BuyConsumableRequest>
{
    public override async Task<ASObject> HandlePacket(BuyConsumableRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName) ?? throw new Exception($"Can't find game item {request.ItemName}");

        if (gameItem.Cash > 0)
            player.RemoveCash(gameItem.Cash.Value * request.Amount);
        else if (gameItem.Cost > 0)
            player.RemoveCoins(gameItem.Cost.Value * request.Amount);
        
        player.AddItem(request.ItemName, request.Amount);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyConsumableRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public int Amount { get; set; }
}

public class BuyConsumableValidator : AbstractValidator<BuyConsumableRequest>
{
    public BuyConsumableValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
