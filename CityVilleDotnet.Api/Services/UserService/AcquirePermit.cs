using CityVilleDotnet.Api.Common.Amf;
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
    public override async Task<ASObject> HandlePacket(AcquirePermitRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null) throw new Exception($"Game item {request.ItemName} not found");
        if (gameItem.Unlock is null) throw new Exception($"Game item {request.ItemName} doesn't have unlock defined");

        var player = await context.Set<User>()
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Where(x => x.UserId == userId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        var permitCost = player.GetNextPermitCost();
        var permitData = player.GetExpansionData();

        if (permitData is null) throw new Exception("Can't find permit data");

        if (player.Cash < permitCost)
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        player.RemoveCash(permitCost);
        player.AddItem("permits", permitData[1]);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject { { "itemName", request.ItemName } });
    }
}

public class AcquirePermitRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}

public class AcquirePermitValidator : AbstractValidator<AcquirePermitRequest>
{
    public AcquirePermitValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}