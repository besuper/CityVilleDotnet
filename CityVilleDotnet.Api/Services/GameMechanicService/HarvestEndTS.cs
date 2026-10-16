using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Api.Pages;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public sealed class HarvestEndTS(CityVilleDbContext context, IConfiguration configuration) : AmfService<HarvestEndTSRequest>
{
    public override async Task<ASObject> HandlePacket(HarvestEndTSRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var globalTableProviders = GameSettingsManager.Instance.GetGlobalTableProviders();
        var hubNames = GameSettingsManager.Instance.GetPreyGroups().SelectMany(x => x.GetHubNames()).ToList();
        var experiments = (configuration.GetSection("experiments").Get<List<Experiment>>() ?? []).ToDictionary(x => x.Name, x => x.Variant);

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId || globalTableProviders.Contains(o.ItemName) || hubNames.Contains(o.ItemName)))
            .Include(x => x.InventoryItems)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.GlobalTableOverrides)
            .Include(x => x.FeatureRollCounters)
            .Include(x => x.PreyGroups)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.ObjectId);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        var mechanic = gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("harvestEndTS")
                       ?? throw new Exception($"No harvestEndTS mechanic found for {obj.ItemName} in game mode {request.GameMode}");

        var duration = mechanic.Duration > 0 ? (int)mechanic.Duration : gameItem.HarvestLoopConfig?.TimerDuration ?? 0;

        obj.HarvestEndTimer(duration, mechanic.UpgradeActionMechanic);

        if (gameItem.EnergyCost?.Harvest is not null)
            player.RemoveEnergy(int.Parse(gameItem.EnergyCost.Harvest));

        player.CollectDoobersRewards(obj.GetItemName());
        player.RollPreySpawns(world, gameItem, experiments);

        player.HandleQuestsProgress("harvestByClass", className: obj.GetClassName().ToString());
        player.HandleQuestsProgress("harvestByKeyword", itemName: obj.GetItemName());
        player.HandleQuestsProgress("harvestItemByName", itemName: obj.ItemName);
        player.HandleQuestsProgress("harvestResidenceByName", itemName: obj.ItemName);
        player.HandleQuestsProgress("harvestResidenceByRegEx", itemName: obj.ItemName);
        player.HandleQuestsProgress("harvestBusinessByName", itemName: obj.ItemName);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class HarvestEndTSRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
}
