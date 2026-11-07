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
    public override async Task<ASObject> HandlePacket(SupplyRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId || o.StoredInId == request.ObjectId))
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId || o.StoredInId == request.ObjectId))
            .ThenInclude(x => x.SupplyResources)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.ObjectId);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.IsStorageSupply())
        {
            var stored = world.GetStoredObjects(obj);

            player.SupplyStorage(stored);
            obj.OpenStorageSupply(stored);
        }
        else
        {
            player.ProcessGoods(gameItem);
            obj.OpenBusiness(player.Level);
        }

        player.HandleQuestsProgress("openBusinessByClass", className: obj.GetClassName().ToString());
        player.HandleQuestsProgress("openBusinessByKeyword", itemName: obj.GetItemName());
        player.HandleQuestsProgress("countSupplyItemByName", itemName: obj.GetItemName());

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class SupplyRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
}