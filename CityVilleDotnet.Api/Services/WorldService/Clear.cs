using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Clear(CityVilleDbContext context) : AmfService<ClearRequest>
{
    public override async Task<ASObject> HandlePacket(ClearRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var globalTableProviders = GameSettingsManager.Instance.GetGlobalTableProviders();

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => (o.X == request.Building.Position.X && o.Y == request.Building.Position.Y) || globalTableProviders.Contains(o.ItemName)))
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception($"Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.EnergyCost?.Clear is not null)
            player.RemoveEnergy(int.Parse(gameItem.EnergyCost.Clear));

        var secureRands = player.CollectDoobersRewards(obj.ItemName);

        world.RemoveBuilding(obj);

        if (obj.FranchiseLocation is not null && obj.ItemOwner is not null)
        {
            var sender = await context.Set<Player>()
                .Include(x => x.Franchises)
                .ThenInclude(x => x.Locations)
                .FirstOrDefaultAsync(x => x.Snuid.ToString() == obj.ItemOwner, cancellationToken);

            if (sender is not null)
                sender.RemoveFranchiseLocation(player.Snuid.ToString(), obj.WorldFlatId);
        }

        context.Set<WorldObject>().Remove(obj);

        player.HandleQuestsProgress("clearByClass", className: obj.ClassName.ToString()); // Wilderness
        player.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["secureRands"] = AmfConverter.Convert(secureRands)
        });
    }
}

public class ClearRequest
{
    [AmfParam(1)] public BuildingClearRequest Building { get; set; } = new();
}

public class BuildingClearRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}