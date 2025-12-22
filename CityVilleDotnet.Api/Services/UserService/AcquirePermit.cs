using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class AcquirePermit(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var itemName = @params[0] as string;

        if (string.IsNullOrEmpty(itemName)) throw new Exception("Item name can't be null or empty");

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is null) throw new Exception($"Game item {itemName} not found");
        if (gameItem.Unlock is null) throw new Exception($"Game item {itemName} doesn't have unlock defined");

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
        player.AddItem(gameItem.Unlock, permitData[1]);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject { { "itemName", itemName } });
    }
}