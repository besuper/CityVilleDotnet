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

public class BuyEnergy(CityVilleDbContext context) : AmfService<BuyEnergyRequest>
{
    public override async Task<ASObject> HandlePacket(BuyEnergyRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null) throw new Exception($"Game item {request.ItemName} not found");
        if (gameItem.Cash is null || gameItem.EnergyRewards is null) throw new Exception($"Game item {request.ItemName} doesn't have cash or energy reward");

        var player = await context.Set<Player>()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.StreakLength > 0))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't find player");

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

public class BuyEnergyValidator : AbstractValidator<BuyEnergyRequest>
{
    public BuyEnergyValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}