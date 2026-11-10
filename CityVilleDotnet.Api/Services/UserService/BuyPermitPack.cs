using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class BuyPermitPack(CityVilleDbContext context) : AmfService<BuyPermitPackRequest>
{
    public override async Task<ASObject> HandlePacket(BuyPermitPackRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName) ?? throw new Exception($"Can't find game item {request.ItemName}");

        if (gameItem.Type?.StartsWith("permit_pack") != true)
        {
            throw new Exception($"Game item {request.ItemName} is not a permit pack");
        }

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

public class BuyPermitPackRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}

public class BuyPermitPackValidator : AbstractValidator<BuyPermitPackRequest>
{
    public BuyPermitPackValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}
