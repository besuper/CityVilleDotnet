using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Finish(CityVilleDbContext context) : AmfService<FinishRequest>
{
    public override async Task<ASObject> HandlePacket(FinishRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
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

        world.CalculatePopulation();

        player.HandleQuestsProgress(""); // Empty actionType to force recheck counts
        player.HandleQuestsProgress("finishConstructionByName", itemName: obj.GetItemName());
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