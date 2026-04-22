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
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception($"Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item with name {obj.ItemName}");

        if (gameItem.CommodityRequired is null)
            throw new Exception($"Game item {obj.ItemName} doesn't have commodity required");

        if (player.Goods < gameItem.CommodityRequired)
            // TODO: Show an error ?
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        if (gameItem.EnergyCost?.Open is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Open);

            if (!player.RemoveEnergy(energyCost))
            {
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
            }
        }

        player.RemoveGoods(gameItem.CommodityRequired.Value);

        obj.OpenBusiness();

        player.HandleQuestsProgress("openBusinessByClass", className: obj.GetClassName().ToString());
        player.HandleQuestsProgress("openBusinessByName", itemName: obj.GetItemName());
        player.HandleQuestsProgress("openBusinessByKeyword", itemName: obj.GetItemName());
        player.CheckCompletedQuests();

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