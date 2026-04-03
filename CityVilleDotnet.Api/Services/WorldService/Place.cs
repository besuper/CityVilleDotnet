using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Place(CityVilleDbContext context, ILogger<Place> logger) : AmfService<PlaceRequest>
{
    public override async Task<ASObject> HandlePacket(PlaceRequest request, Guid userId, CancellationToken cancellationToken)
    {
        logger.LogDebug("Received place action {@PlaceRequest}", request);

        // TODO: Implement components
        // ignore components for now

        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Include(x => x.Quests.OrderBy(q => q.Order))
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Masteries)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (user.Player is null) throw new Exception($"User not found with id {userId}");

        var world = user.GetPlayer().GetWorld();

        var gameItem = GameSettingsManager.Instance.GetItem(request.Building.ItemName);

        if (gameItem is null)
            throw new Exception("Can't build building not registered in XML file");

        // Handle macro buildings (ExplodableMacroObjectMechanic)
        var explodeToRect = gameItem.GetExplodeToRect();

        if (explodeToRect is not null)
        {
            var worldRect = GameSettingsManager.Instance.GetWorldRect(explodeToRect);

            if (worldRect is not null)
            {
                foreach (var rectObj in worldRect.Objects.Objects)
                {
                    var childItem = GameSettingsManager.Instance.GetItem(rectObj.ItemName);

                    if (childItem is null) continue;

                    var childClassName = Enum.Parse<BuildingClassType>(childItem.Type.Pascalize());

                    var childObj = new WorldObject(
                        rectObj.ItemName,
                        childClassName,
                        null,
                        false,
                        0,
                        WorldObjectState.Static,
                        rectObj.Direction,
                        null,
                        null,
                        request.Building.Position.X + rectObj.X,
                        request.Building.Position.Y + rectObj.Y,
                        request.Building.Position.Z,
                        1
                    );

                    if (rectObj.UseConstructionSite == "true" && childItem.Construction is not null)
                    {
                        var csItem = GameSettingsManager.Instance.GetItem(childItem.Construction);
                        if (csItem is not null)
                            childObj.SetAsConstructionSite(childItem.Construction, csItem.NumberOfStages ?? 0);
                    }

                    childObj.UpdateWorldFlatId(world.GetAvailableBuildingId());
                    world.AddBuilding(childObj);
                }

                if (user.Player!.HasItem(request.Building.ItemName))
                {
                    var removedItem = user.Player.RemoveItem(request.Building.ItemName);

                    if (removedItem is not null)
                        context.Set<InventoryItem>().Remove(removedItem);
                }
                else if (gameItem.Cost is not null)
                {
                    user.Player!.RemoveCoins(gameItem.Cost.Value);
                }

                user.HandleQuestsProgress("placeByClass", className: request.Building.ClassName.ToString());
                user.HandleQuestsProgress("placeBuildingByName", itemName: request.Building.ItemName);
                user.HandleQuestsProgress("placeByKeyword", itemName: request.Building.ItemName);
                user.CheckCompletedQuests();

                await context.SaveChangesAsync(cancellationToken);

                return new CityVilleResponse().MetaData(new ASObject
                {
                    ["QuestComponent"] = AmfConverter.Convert(user.Quests.Select(x => x.ToDto()))
                });
            }
        }

        var objId = world.GetAvailableBuildingId();

        var obj = new WorldObject(
            request.Building.ItemName,
            request.Building.ClassName,
            null,
            request.Building.Deleted,
            request.Building.TempId,
            request.Building.State,
            request.Building.Direction,
            ServerUtils.GetCurrentTime(),
            ServerUtils.GetCurrentTime(),
            request.Building.Position.X,
            request.Building.Position.Y,
            request.Building.Position.Z,
            objId
        );

        if (gameItem.Construction is not null)
        {
            var constructionItem = GameSettingsManager.Instance.GetItem(gameItem.Construction);

            if (constructionItem?.NumberOfStages is null)
                throw new Exception($"Construction item not found with {gameItem.Construction}");

            obj.SetAsConstructionSite(gameItem.Construction, constructionItem.NumberOfStages.Value);
        }

        world.AddBuilding(obj);

        if (user.Player!.HasItem(request.Building.ItemName))
        {
            var removedItem = user.Player.RemoveItem(request.Building.ItemName);

            if (removedItem is not null)
                context.Set<InventoryItem>().Remove(removedItem);
        }
        else
        {
            if (gameItem.Cost is not null)
                user.Player!.RemoveCoins(gameItem.Cost.Value);
        }

        // Set TempId to current clientId to fix harvest
        if (request.Building.ClassName == BuildingClassType.Business)
        {
            obj.SetTempId(request.Building.Id);
        }

        // TODO: Check coins, goods, energy, etc...
        // Add population

        user.HandleQuestsProgress("placeByClass", className: request.Building.ClassName.ToString());
        user.HandleQuestsProgress("placeBuildingByName", itemName: request.Building.ItemName);
        user.HandleQuestsProgress("placeByKeyword", itemName: request.Building.ItemName);
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().MetaData(new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.Select(x => x.ToDto()))
        }).Data(new ASObject
        {
            { "id", obj.WorldFlatId }
        });
    }
}

public class PlaceRequest
{
    [AmfParam(1)] public BuildingPlaceRequest Building { get; set; } = new();
}

public class BuildingPlaceRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
    [AmfParam("className")] public BuildingClassType ClassName { get; set; }
    [AmfParam("state")] public WorldObjectState State { get; set; }
    [AmfParam("itemName")] public string ItemName { get; set; } = string.Empty;
    [AmfParam("deleted")] public bool Deleted { get; set; }
    [AmfParam("direction")] public int Direction { get; set; }
    [AmfParam("tempId")] public int TempId { get; set; }
    [AmfParam("id")] public int Id { get; set; }
}

public class PlaceValidator : AbstractValidator<PlaceRequest>
{
    public PlaceValidator()
    {
        RuleFor(x => x.Building.ItemName).NotEmpty().MaximumLength(64);
    }
}