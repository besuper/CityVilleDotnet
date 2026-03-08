using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyCoins(CityVilleDbContext context) : AmfService<BuyCoinsRequest>
{
    public override async Task<ASObject> HandlePacket(BuyCoinsRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null) throw new Exception($"User not found with id {userId}");

        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null) throw new Exception($"Game item {request.ItemName} not found");

        if (gameItem.CoinRewards is null)
            throw new Exception($"Game item {request.ItemName} does not have coinRewards");

        if (gameItem.Cost is not null && gameItem.Cost > 0)
        {
            if (player.Gold < gameItem.Cost)
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

            player.RemoveCoins(gameItem.Cost.Value);
        }
        else if (gameItem.Cash is not null && gameItem.Cash > 0)
        {
            if (player.Cash < gameItem.Cash)
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

            player.RemoveCash(gameItem.Cash.Value);
        }

        player.AddCoins(gameItem.CoinRewards.Value);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyCoinsRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}