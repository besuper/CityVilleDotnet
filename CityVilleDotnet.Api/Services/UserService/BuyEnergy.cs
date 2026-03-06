using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyEnergy(CityVilleDbContext context) : AmfService<BuyEnergyRequest>
{
    public override async Task<ASObject> HandlePacket(BuyEnergyRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null) throw new Exception($"Game item {request.ItemName} not found");

        if (gameItem.Cash is null || gameItem.EnergyRewards is null)
            throw new Exception($"Game item {request.ItemName} does defineds not have cash or energy reward");

        if (player.Cash < gameItem.Cash)
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        player.RemoveCash(gameItem.Cash.Value);
        player.AddEnergy(gameItem.EnergyRewards.Value);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyEnergyRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}