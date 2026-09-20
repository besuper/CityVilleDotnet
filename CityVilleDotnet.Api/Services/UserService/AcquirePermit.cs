using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class AcquirePermit(CityVilleDbContext context) : AmfService<AcquirePermitRequest>
{
    private const string PermitName = "permits";

    public override async Task<ASObject> HandlePacket(AcquirePermitRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null) throw new Exception($"Game item {request.ItemName} not found");
        if (gameItem.Unlock is null) throw new Exception($"Game item {request.ItemName} doesn't have unlock defined");

        var permitItem = GameSettingsManager.Instance.GetItem(PermitName);

        if (permitItem?.Cash is null) throw new Exception($"Game item {PermitName} doesn't have cash defined");

        var player = await context.Set<Player>()
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        var permitCost = permitItem.Cash.Value * request.Count;

        if (player.Cash < permitCost)
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        if (permitItem.InventoryLimit > 0 && player.CountInventoryItem(PermitName) + request.Count > permitItem.InventoryLimit)
            return new CityVilleResponse().Error(GameErrorType.InvalidData);

        player.RemoveCash(permitCost);
        player.AddItem(PermitName, request.Count);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject { { "itemName", request.ItemName } });
    }
}

public class AcquirePermitRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public int Count { get; set; }
}

public class AcquirePermitValidator : AbstractValidator<AcquirePermitRequest>
{
    public AcquirePermitValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Count).GreaterThan(0);
    }
}
