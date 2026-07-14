using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public sealed class Finish(CityVilleDbContext context) : AmfService<FinishRequest>
{
    public override async Task<ASObject> HandlePacket(FinishRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.StorageItems)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.Slots)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.MapRects)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception($"Can't find building with ID {request.Building.Id}");

        if (obj.Builds is null)
            throw new Exception($"Can't find `builds` {obj}");

        var constructionItemName = obj.ItemName;

        var createdObjects = obj.FinishConstruction();

        foreach (var newObject in createdObjects)
        {
            newObject.UpdateWorldFlatId(world.GetAvailableBuildingId());
            world.AddBuilding(newObject);
        }

        var finishedItem = GameSettingsManager.Instance.GetItem(obj.GetItemName());

        if (finishedItem is not null)
        {
            world.GrantFreeExpansions(finishedItem.GrantedExpansionsOnFinish, finishedItem.GrantedExpansionType);

            foreach (var consumed in player.ConsumeInventoryGate(finishedItem, "build"))
                context.Set<InventoryItem>().Remove(consumed);
        }

        if (obj.GetClassName() == BuildingClassType.ZooEnclosure)
            obj.GrantInitialZooAnimal(player.Snuid);

        world.CalculatePopulation();
        
        if(finishedItem?.Population?.Min is not null)
            player.HandleQuestsProgress("incrementalPopulationCount", amount: finishedItem.Population.Min.Value);

        player.HandleQuestsProgress(""); // Empty actionType to force recheck counts
        player.HandleQuestsProgress("finishConstructionByName", itemName: obj.GetItemName());
        player.HandleQuestsProgress("finishConstructionByKeyword", itemName: obj.GetItemName());
        player.HandleQuestsProgress("finishConstructionByClass", className: obj.GetClassName().ToString());
        player.CheckCompletedQuests();

        player.CollectDoobersRewards(constructionItemName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["id"] = obj.WorldFlatId
        });
    }
}

public class FinishRequest
{
    [AmfParam(1)] public BuildingFinishRequest Building { get; set; } = new();
}

public class BuildingFinishRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
    [AmfParam("id")] public int Id { get; set; }
}