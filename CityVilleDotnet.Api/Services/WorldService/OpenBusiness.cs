using CityVilleDotnet.Api.Common.Amf;
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
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.Building.Id || o.TempId == request.Building.Id))
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.Building.Id);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item with name {obj.ItemName}");

        player.ProcessGoods(gameItem, request.Params.FirstOrDefault() ?? "goods");

        obj.OpenBusiness(player.Level);

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
    [AmfParam(3)] public string[] Params { get; set; } = [];
}

public class BuildingOpenBusiness
{
    [AmfParam("id")] public int Id { get; set; }
}