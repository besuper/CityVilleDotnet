using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyCoins(CityVilleDbContext context) : AmfService<BuyCoinsRequest>
{
    public override async Task<ASObject> HandlePacket(BuyCoinsRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null) throw new Exception($"Game item {request.ItemName} not found");
        if (gameItem.CoinRewards is null) throw new Exception($"Game item {request.ItemName} does not have coinRewards");

        var player = await context.Set<Player>().FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("User not found");

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

public class BuyCoinsValidator : AbstractValidator<BuyCoinsRequest>
{
    public BuyCoinsValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}