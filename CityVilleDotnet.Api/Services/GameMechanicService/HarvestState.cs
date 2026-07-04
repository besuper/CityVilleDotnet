using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

internal sealed class HarvestState(CityVilleDbContext context) : AmfService<HarvestStateRequest>
{
    public override async Task<ASObject> HandlePacket(HarvestStateRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .Include(x => x.InventoryItems)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.EnergyCost?.Harvest is not null)
            player.RemoveEnergy(int.Parse(gameItem.EnergyCost.Harvest));

        obj.Harvest();
        player.CollectDoobersRewards(obj.GetItemName());

        player.HandleQuestsProgress("harvestByClass", className: obj.GetClassName().ToString());
        player.HandleQuestsProgress("harvestBusinessByName", itemName: obj.GetItemName());
        player.HandleQuestsProgress("harvestBusinessByClass", className: obj.GetClassName().ToString());
        player.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class HarvestStateRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
}