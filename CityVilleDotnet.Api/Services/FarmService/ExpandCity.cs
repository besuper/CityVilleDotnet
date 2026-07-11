using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using FluentValidation;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FarmService;

public class ExpandCity(CityVilleDbContext context, ILogger<ExpandCity> logger) : AmfService<ExpandCityRequest>
{
    private const string PermitName = "permits";

    public override async Task<ASObject> HandlePacket(ExpandCityRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var item = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (item is null) throw new Exception($"Can't find item {request.ItemName}");
        if (item.Height is null || item.Width is null) throw new Exception($"Item {request.ItemName} has no height or width defined");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.IncentivizedExpansions)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't find player");

        var permitData = player.GetExpansionData();

        if (permitData is null) throw new Exception("Can't find permit data");

        var requiredPermit = permitData[1];

        if (player.CountInventoryItem(PermitName) < requiredPermit)
            throw new Exception($"You need {requiredPermit} {PermitName} to expand this city");

        if (item.Cost is not null)
            player.RemoveCoins(item.Cost.Value);

        var world = player.GetWorld();

        if (world.MapRects.Any(m => m.X == request.Coordinates.X && m.Y == request.Coordinates.Y))
        {
            logger.LogError("Map expansion already exist {PlayerSnuid} {MapRectsCount} | {CoordinatesX} {CoordinatesY}", player.Snuid, world.MapRects.Count, request.Coordinates.X, request.Coordinates.Y);
            return new CityVilleResponse();
        }

        var newMapRect = new MapRect
        {
            X = request.Coordinates.X,
            Y = request.Coordinates.Y,
            Height = item.Height.Value,
            Width = item.Width.Value
        };

        world.AddMapRect(newMapRect);

        var remappedIds = new List<object>();

        foreach (var tree in request.Trees)
        {
            var newTree = new WorldObject(
                tree.ItemName,
                BuildingClassType.Wilderness,
                null,
                false,
                tree.Id,
                WorldObjectState.Static,
                0,
                ServerUtils.GetCurrentTime(),
                ServerUtils.GetCurrentTime(),
                tree.X,
                tree.Y,
                0,
                world.GetAvailableBuildingId()
            );

            world.AddBuilding(newTree);

            remappedIds.Add(new
            {
                id = tree.Id,
                newId = newTree.WorldFlatId
            });
        }
        
        var finalIds = new List<int>();
        var incentivizedExpansion = world.IncentivizedExpansions.FirstOrDefault(e => e.IsActive() && e.X == request.Coordinates.X && e.Y == request.Coordinates.Y);

        if (incentivizedExpansion is not null)
        {
            var config = GameSettingsManager.Instance.GetDynamicExpansion(incentivizedExpansion.ExpansionId);

            if (config is not null)
            {
                var rewards = config.GetRewards().Where(r => r.Callback == "embedRewardInWorld" && !r.IsInventoryOnly());
                var tempIds = request.TempIds ?? [];

                foreach (var (reward, tempId) in rewards.Zip(tempIds))
                {
                    var rewardObj = world.EmbedDynamicExpansionObject(reward, incentivizedExpansion.X!.Value, incentivizedExpansion.Y!.Value, tempId);

                    finalIds.Add(rewardObj.WorldFlatId);
                }
            }

            incentivizedExpansion.Complete();
        }

        player.IncrementExpansionsPurchased();
        var removedItem = player.RemoveItem(PermitName, requiredPermit);

        if (removedItem is not null)
            context.Set<InventoryItem>().Remove(removedItem);

        player.HandleQuestsProgress("incrementalExpansionCount");
        player.HandleQuestsProgress("expand");

        player.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            { "trees", remappedIds },
            { "finalIds", finalIds },
        });
    }
}

public sealed class ExpandCityRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public ExpandCityCoordinates Coordinates { get; set; } = new();
    [AmfParam(2)] public ExpandCityTree[] Trees { get; set; } = [];
    [AmfParam(3)] public int[]? TempIds { get; set; }
}

public class ExpandCityCoordinates
{
    [AmfParam("x")] public int X { get; set; }
    [AmfParam("y")] public int Y { get; set; }
}

public class ExpandCityTree
{
    [AmfParam("id")] public int Id { get; set; }
    [AmfParam("itemName")] public string ItemName { get; set; } = string.Empty;
    [AmfParam("x")] public int X { get; set; }
    [AmfParam("y")] public int Y { get; set; }
}

public class ExpandCityRequestValidator : AbstractValidator<ExpandCityRequest>
{
    public ExpandCityRequestValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
        RuleForEach(x => x.Trees).ChildRules(tree => { tree.RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64); });
    }
}