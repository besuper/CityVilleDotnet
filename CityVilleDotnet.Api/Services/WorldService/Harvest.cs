using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Pages;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Harvest(CityVilleDbContext context, ILogger<Harvest> logger, IConfiguration configuration) : AmfService<HarvestRequest>
{
    public override async Task<ASObject> HandlePacket(HarvestRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var globalTableProviders = GameSettingsManager.Instance.GetGlobalTableProviders();
        var hubNames = GameSettingsManager.Instance.GetPreyGroups().SelectMany(x => x.GetHubNames()).ToList();
        var experiments = (configuration.GetSection("experiments").Get<List<Experiment>>() ?? []).ToDictionary(x => x.Name, x => x.Variant);

        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => (o.X == request.Building.Position.X && o.Y == request.Building.Position.Y) || globalTableProviders.Contains(o.ItemName) || hubNames.Contains(o.ItemName) || o.ClassName == BuildingClassType.DockHouse))
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => (o.X == request.Building.Position.X && o.Y == request.Building.Position.Y) || globalTableProviders.Contains(o.ItemName) || hubNames.Contains(o.ItemName) || o.ClassName == BuildingClassType.DockHouse))
            .ThenInclude(x => x.Workers)
            .Include(x => x.InventoryItems)
            .Include(x => x.SeenFlags)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Masteries)
            .Include(x => x.GlobalTableOverrides)
            .Include(x => x.FeatureRollCounters)
            .Include(x => x.PreyGroups)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null) throw new Exception("Player not found");

        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception("Can't find building");

        var itemName = obj.GetItemName();

        if (itemName is null)
            throw new Exception("Item name is null, can't harvest");

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {itemName}");

        var isFranchiseHarvest = obj.IsFranchiseSupplied();

        if (!isFranchiseHarvest && !obj.CanHarvest())
            throw new Exception("Building is not harvestable");

        if (!isFranchiseHarvest && !obj.IsTaxiReady && gameItem.EnergyCost?.Harvest is not null)
        {
            user.RemoveEnergy(int.Parse(gameItem.EnergyCost.Harvest));
        }

        var contractName = obj.ContractName;

        int coinYield;
        int cashYield;
        List<int> secureRands;

        if (isFranchiseHarvest)
        {
            (coinYield, cashYield) = obj.HarvestFranchise(user.Level);
            secureRands = user.CollectDoobersRewards(itemName, modifierGroupName: "franchise");
        }
        else
        {
            (coinYield, cashYield, secureRands) = user.HarvestObject(world, obj, ServerUtils.GetActionTime(request.ClientEnqueueTime));
        }

        user.TrackHarvest(world, obj, itemName, contractName, experiments);

        logger.LogDebug("Secure rands {Join}", string.Join(",", secureRands.ToArray()));
        logger.LogDebug("Secure rands {SecureRandsCount}", secureRands.Count);

        var objectPopulation = gameItem.Population?.Min ?? -1;
        var worldPopulation = world.GetCurrentPopulation();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["retCoinYield"] = coinYield,
            ["retCashYield"] = cashYield,
            ["secureRands"] = AmfConverter.Convert(secureRands),
            ["objectPopulation"] = objectPopulation,
            ["worldPopulation"] = worldPopulation
        });
    }
}

public class HarvestRequest
{
    [AmfParam(1)] public BuildingHarvestRequest Building { get; set; } = new();
    [AmfParam(2)] public long? ClientEnqueueTime { get; set; }
}

public class BuildingHarvestRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}