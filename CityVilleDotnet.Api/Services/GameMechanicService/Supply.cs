using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

internal sealed class Supply(CityVilleDbContext context) : AmfService<SupplyRequest>
{
    public override async Task<ASObject> HandlePacket(SupplyRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null) throw new Exception($"User not found with id {userId}");

        var world = user.GetPlayer().GetWorld();

        var obj = world.GetBuildingById(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.CommodityRequired is not null && gameItem.CommodityRequired > 0)
        {
            if (user.Player.Goods < gameItem.CommodityRequired)
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

            user.Player.RemoveGoods(gameItem.CommodityRequired.Value);
        }

        obj.OpenBusiness();

        user.HandleQuestsProgress("openBusinessByName", itemName: obj.ItemName);
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class SupplyRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
}