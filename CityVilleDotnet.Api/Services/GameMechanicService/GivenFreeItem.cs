using System.Collections;
using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class GivenFreeItem(CityVilleDbContext context, ILogger<GivenFreeItem> logger) : AmfService<GivenFreeItemRequest>
{
    public override async Task<ASObject> HandlePacket(GivenFreeItemRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var owner = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        if (owner.GivenFreeItem)
            return GatewayService.CreateEmptyResponse();

        var gameItem = GameSettingsManager.Instance.GetItem(owner.ItemName) ?? throw new Exception($"Can't find game item for {owner.ItemName}");

        var mechanic = gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("givenFreeItem")
                       ?? throw new Exception($"No givenFreeItem mechanic found for {owner.ItemName} in game mode {request.GameMode}");

        if (mechanic.FreeItem is null)
            throw new Exception($"No freeItem defined on givenFreeItem mechanic for {owner.ItemName}");

        var freeGameItem = GameSettingsManager.Instance.GetItem(mechanic.FreeItem) ?? throw new Exception($"Can't find game item for {mechanic.FreeItem}");

        logger.LogDebug("GivenFreeItem item={Name} mechanic={FreeItem}", freeGameItem.Name, mechanic.FreeItem);
        
        var tempId = -1;

        if (request.ExtraData.TryGetValue("tempID", out var tempIdMap) && tempIdMap is IDictionary dict && dict.Contains("freeObj") && dict["freeObj"] is not null)
            tempId = Convert.ToInt32(dict["freeObj"]);

        if (tempId == -1)
        {
            player.AddItem(mechanic.FreeItem);
            
            logger.LogWarning("GivenFreeItem tempId is -1, sent to inventory {MechanicFreeItem}", mechanic.FreeItem);
        }
        else
        {
            var obj = new WorldObject(
                mechanic.FreeItem,
                Enum.Parse<BuildingClassType>(freeGameItem.Type.Pascalize()),
                null,
                false,
                tempId,
                WorldObjectState.Static,
                0,
                ServerUtils.GetCurrentTime(),
                ServerUtils.GetCurrentTime(),
                owner.X + mechanic.XOffset,
                owner.Y + mechanic.YOffset,
                owner.Z ?? 0,
                world.GetAvailableBuildingId()
            );

            world.AddBuilding(obj);
            
            logger.LogDebug("Created new given free item in world {ObjWorldFlatId} {ObjX} {ObjY}", obj.WorldFlatId, obj.X, obj.Y);
        }

        owner.MarkFreeItemGiven();
        
        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class GivenFreeItemRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
    [AmfParam(3)] public Dictionary<string, object> ExtraData { get; set; } = new();
}
