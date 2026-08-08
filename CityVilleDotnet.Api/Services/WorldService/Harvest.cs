using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Harvest(CityVilleDbContext context, ILogger<HarvestRequest> logger) : AmfService<HarvestRequest>
{
    public override async Task<ASObject> HandlePacket(HarvestRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var globalTableProviders = GameSettingsManager.Instance.GetGlobalTableProviders();

        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => (o.X == request.Building.Position.X && o.Y == request.Building.Position.Y) || globalTableProviders.Contains(o.ItemName)))
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => (o.X == request.Building.Position.X && o.Y == request.Building.Position.Y) || globalTableProviders.Contains(o.ItemName)))
            .ThenInclude(x => x.Workers)
            .Include(x => x.InventoryItems)
            .Include(x => x.SeenFlags)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Masteries)
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

        var className = obj.GetClassName();

        var isFranchiseHarvest = obj.IsFranchiseSupplied();

        if (!isFranchiseHarvest && !obj.CanHarvest())
            throw new Exception("Building is not harvestable");

        if (gameItem.EnergyCost?.Harvest is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Harvest);

            if (!user.RemoveEnergy(energyCost))
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
        }

        var contractName = obj.ContractName;
        var hasContract = obj.ContractName is not null;

        int coinYield;
        int cashYield;
        List<int> secureRands;

        if (isFranchiseHarvest)
        {
            (coinYield, cashYield) = obj.HarvestFranchise();
            secureRands = user.CollectDoobersRewards(itemName, modifierGroupName: "franchise");
        }
        else
        {
            var coinMultiplier = className.IsBusiness() ? Math.Max(obj.Visits ?? 0, 1) : 1;

            var premiumGoodsMultiplier = 1.0;
            var workerBonus = gameItem.GetWorkerHarvestBonus();

            if (workerBonus?.Field == "premium_goods" && obj.Workers.Count > 0)
                premiumGoodsMultiplier += obj.Workers.Count * workerBonus.PercentModifier / 100.0;

            (coinYield, cashYield) = obj.Harvest(ServerUtils.GetActionTime(request.ClientEnqueueTime));
            secureRands = user.CollectDoobersRewards(itemName, coinMultiplier: coinMultiplier, premiumGoodsMultiplier: premiumGoodsMultiplier);
        }

        logger.LogDebug("Secure rands {Join}", string.Join(",", secureRands.ToArray()));
        logger.LogDebug("Secure rands {SecureRandsCount}", secureRands.Count);

        user.HandleQuestsProgress("harvestByClass", className: className.ToString());
        user.HandleQuestsProgress("harvestByKeyword", itemName: itemName); 
        user.HandleQuestsProgress("harvestResidenceByName", itemName: obj.ItemName);
        user.HandleQuestsProgress("harvestResidenceByRegEx", itemName: obj.ItemName);
        user.HandleQuestsProgress("harvestItemByName", itemName: obj.ItemName);

        if (contractName is not null)
        {
            user.HandleQuestsProgress("harvestContractByName", itemName: contractName);
        }

        if (obj.ClassName == BuildingClassType.Plot || hasContract)
        {
            user.HandleQuestsProgress("harvestPlotByName", itemName: itemName);

            if (gameItem.HasMasteries())
                user.IncrementMastery(gameItem.Name);
        }

        if (obj.ClassName == BuildingClassType.Business)
        {
            user.HandleQuestsProgress("harvestBusinessByName", itemName: itemName);
            user.HandleQuestsProgress("harvestBusinessByClass", className: className.ToString());
            user.HandleQuestsProgress("harvestBusinessByKeyword", itemName: itemName);
        }

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