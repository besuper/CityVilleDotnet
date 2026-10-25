using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyGenericBundle(CityVilleDbContext context) : AmfService<BuyGenericBundleRequest>
{
    public override async Task<ASObject> HandlePacket(BuyGenericBundleRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName) ?? throw new Exception($"Can't find game item {request.ItemName}");

        if (gameItem.Type != "genericBundle") throw new Exception($"Game item {request.ItemName} is not a generic bundle");

        var player = await context.Set<Player>()
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.RemoveCash(gameItem.Cash ?? 0);

        foreach (var reward in gameItem.ItemRewards)
        {
            player.AddItem(reward.Name, reward.Quantity);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class BuyGenericBundleRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}

public class BuyGenericBundleValidator : AbstractValidator<BuyGenericBundleRequest>
{
    public BuyGenericBundleValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}
