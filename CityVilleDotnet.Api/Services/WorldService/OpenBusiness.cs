using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class OpenBusiness(CityVilleDbContext context) : AmfService<OpenBusinessRequest>
{
    public override async Task<ASObject> HandlePacket(OpenBusinessRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Player.Id == playerId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (user.Player is null) throw new Exception("Player not found");

        var world = user.GetPlayer().GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception($"Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item with name {obj.ItemName}");

        if (gameItem.CommodityRequired is null)
            throw new Exception($"Game item {obj.ItemName} doesn't have commodity required");

        if (user.Player!.Goods < gameItem.CommodityRequired)
            // TODO: Show an error ?
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        if (gameItem.EnergyCost?.Open is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Open);

            if (!user.Player!.RemoveEnergy(energyCost))
            {
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
            }
        }

        user.Player.RemoveGoods(gameItem.CommodityRequired.Value);

        obj.OpenBusiness();

        user.HandleQuestsProgress("openBusinessByClass", className: obj.GetClassName().ToString());
        user.HandleQuestsProgress("openBusinessByName", itemName: obj.GetItemName());
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class OpenBusinessRequest
{
    [AmfParam(1)] public BuildingOpenBusiness Building { get; set; } = new();
}

public class BuildingOpenBusiness
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}