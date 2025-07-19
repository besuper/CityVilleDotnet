using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyEnergy(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var itemName = @params[0] as string;

        if (string.IsNullOrEmpty(itemName)) throw new Exception("Item name can't be null or empty");

        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is null) throw new Exception($"Game item {itemName} not found");

        if (gameItem.Cash is null || gameItem.EnergyRewards is null)
            throw new Exception($"Game item {itemName} does not have cash or energy rewards defined");

        if (player.Cash < gameItem.Cash)
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
        
        player.RemoveCash(gameItem.Cash.Value);
        player.AddEnergy(gameItem.EnergyRewards.Value);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}