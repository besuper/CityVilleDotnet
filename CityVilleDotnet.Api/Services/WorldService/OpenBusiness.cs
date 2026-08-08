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
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(o => o.X == request.Building.Position.X && o.Y == request.Building.Position.Y))
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception($"Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item with name {obj.ItemName}");

        player.ProcessGoods(gameItem);

        if (gameItem.EnergyCost?.Open is not null)
            player.RemoveEnergy(int.Parse(gameItem.EnergyCost.Open));

        obj.OpenBusiness();

        player.HandleQuestsProgress("openBusinessByClass", className: obj.GetClassName().ToString());
        player.HandleQuestsProgress("openBusinessByName", itemName: obj.GetItemName());
        player.HandleQuestsProgress("openBusinessByKeyword", itemName: obj.GetItemName());

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