using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Finish(CityVilleDbContext context) : AmfService<FinishRequest>
{
    public override async Task<ASObject> HandlePacket(FinishRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active).OrderBy(q => q.Order))
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (user.Player is null) throw new Exception("Player not found for user");

        var world = user.GetWorld();

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

        user.HandleQuestsProgress(""); // Empty actionType to force recheck counts
        user.CheckCompletedQuests();

        user.Player!.CollectDoobersRewards(constructionItemName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().MetaData(new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.Select(x => x.ToDto()))
        }).Data(new ASObject
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