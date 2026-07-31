using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyGoods(CityVilleDbContext context) : AmfService<BuyGoodsRequest>
{
    public override async Task<ASObject> HandlePacket(BuyGoodsRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null) throw new Exception($"Game item {request.ItemName} not found");
        if (gameItem.GoodsReward is null) throw new Exception($"Game item {request.ItemName} does not have goodsReward");

        var commodityName = gameItem.GetDefaultCommodityName();

        if (commodityName is null) throw new Exception($"Game item {request.ItemName} does not have a default commodity");

        var player = await context.Set<Player>()
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        if (request.IsGift)
        {
            var removedItem = player.RemoveItem(request.ItemName);

            if (removedItem is not null)
                context.Set<InventoryItem>().Remove(removedItem);
        }
        else if (gameItem.Cost > 0)
        {
            player.RemoveCoins(gameItem.Cost.Value);
        }
        else if (gameItem.Cash > 0)
        {
            player.RemoveCash(gameItem.Cash.Value);
        }

        switch (commodityName)
        {
            case "goods":
                player.AddGoods(gameItem.GoodsReward.Value);
                break;
            case "premium_goods":
                player.AddPremiumGoods(gameItem.GoodsReward.Value);
                break;
            default:
                throw new Exception($"Unknown commodity {commodityName} for game item {request.ItemName}");
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyGoodsRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public bool IsGift { get; set; }
}

public class BuyGoodsValidator : AbstractValidator<BuyGoodsRequest>
{
    public BuyGoodsValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}
