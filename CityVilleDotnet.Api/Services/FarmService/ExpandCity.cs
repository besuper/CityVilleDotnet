using CityVilleDotnet.Api.Common.Amf;
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
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects.Where(m => m.X == request.Coordinates.X && m.Y == request.Coordinates.Y))
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't find player");

        var permitData = player.GetExpansionData();

        if (permitData is null) throw new Exception("Can't find permit data");

        var requiredPermit = permitData[1];

        if (player.CountInventoryItem(PermitName) < requiredPermit)
            throw new Exception($"You need {requiredPermit} {PermitName} to expand this city");

        var world = player.GetWorld();

        if (world.MapRects.Count > 0)
        {
            logger.LogError("Map expansion already exist {PlayerSnuid} {MapRectsCount} | {CoordinatesX} {CoordinatesY}", player.Snuid, world.MapRects.Count, request.Coordinates.X, request.Coordinates.Y);
            return new CityVilleResponse();
        }

        var newMapRect = new MapRect
        {
            X = request.Coordinates.X,
            Y = request.Coordinates.Y,
            Height = int.Parse(item.Height), // FIXME: Change these value to the right type when loading the settings
            Width = int.Parse(item.Width)
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
                -1,
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

        player.IncrementExpansionsPurchased();
        var removedItem = player.RemoveItem(PermitName, requiredPermit);

        if (removedItem is not null)
            context.Set<InventoryItem>().Remove(removedItem);

        player.HandleQuestsProgress("incrementalExpansionCount");
        player.HandleQuestsProgress("expand");

        player.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(remappedIds);
    }
}

public sealed class ExpandCityRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public ExpandCityCoordinates Coordinates { get; set; } = new();
    [AmfParam(2)] public ExpandCityTree[] Trees { get; set; } = [];
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