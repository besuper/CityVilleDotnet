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

public class Explode(CityVilleDbContext context, ILogger<Explode> logger) : AmfService<ExplodeRequest>
{
    public override async Task<ASObject> HandlePacket(ExplodeRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(w => w.WorldFlatId == request.ObjectId || w.TempId == request.ObjectId))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var owner = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(owner.ItemName) ?? throw new Exception($"Can't find game item for {owner.ItemName}");

        var mechanic = gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("explode")
                       ?? throw new Exception($"No explode mechanic found for {owner.ItemName} in game mode {request.GameMode}");

        if (mechanic.ExplodeToRect is null)
            throw new Exception($"No explodeToRect defined on explode mechanic for {owner.ItemName}");

        var worldRect = GameSettingsManager.Instance.GetWorldRect(mechanic.ExplodeToRect)
                        ?? throw new Exception($"Can't find world rect {mechanic.ExplodeToRect}");

        IDictionary? tempIds = null;

        if (request.ExtraData.TryGetValue("tempIds", out var tempIdsMap) && tempIdsMap is IDictionary dict)
            tempIds = dict;

        foreach (var rectObj in worldRect.Objects.Objects)
        {
            var childItem = GameSettingsManager.Instance.GetItem(rectObj.ItemName);

            if (childItem is null)
            {
                logger.LogWarning("Can't find world rect item {ItemName}", rectObj.ItemName);
                continue;
            }

            var tempId = tempIds is not null && tempIds.Contains(rectObj.Id) && tempIds[rectObj.Id] is not null
                ? Convert.ToInt32(tempIds[rectObj.Id])
                : -1;

            var childObj = new WorldObject(
                rectObj.ItemName,
                Enum.Parse<BuildingClassType>(childItem.Type.Pascalize()),
                null,
                false,
                tempId,
                WorldObjectState.Static,
                rectObj.Direction,
                ServerUtils.GetCurrentTime(),
                ServerUtils.GetCurrentTime(),
                owner.X + rectObj.X,
                owner.Y + rectObj.Y,
                owner.Z ?? 0,
                world.GetAvailableBuildingId()
            );

            if (rectObj.UseConstructionSite == "true" && childItem.Construction is not null)
            {
                var csItem = GameSettingsManager.Instance.GetItem(childItem.Construction);

                if (csItem is not null)
                    childObj.SetAsConstructionSite(childItem.Construction, csItem.NumberOfStages ?? 0);
            }

            world.AddBuilding(childObj);
        }

        world.RemoveBuilding(owner);
        context.Remove(owner);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class ExplodeRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
    [AmfParam(3)] public ASObject ExtraData { get; set; } = [];
}
