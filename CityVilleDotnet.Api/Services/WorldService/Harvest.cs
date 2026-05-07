using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
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
        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.X == request.Building.Position.X && o.Y == request.Building.Position.Y))
            .ThenInclude(x => x.FranchiseLocation)
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

        if (!obj.CanHarvest())
            throw new Exception("Building is not harvestable");

        if (gameItem.EnergyCost?.Harvest is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Harvest);

            if (!user.RemoveEnergy(energyCost))
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
        }

        var hasContract = obj.ContractName is not null;

        var coinMultiplier = className.IsBusiness() ? Math.Max(obj.Visits ?? 0, 1) : 1;
        var (coinYield, cashYield) = obj.Harvest();
        var secureRands = user.CollectDoobersRewards(itemName, coinMultiplier: coinMultiplier);

        logger.LogDebug("Secure rands {Join}", string.Join(",", secureRands.ToArray()));
        logger.LogDebug("Secure rands {SecureRandsCount}", secureRands.Count);

        user.HandleQuestsProgress("harvestByClass", className: className.ToString());
        user.HandleQuestsProgress("harvestByKeyword", itemName: itemName);
        user.HandleQuestsProgress("harvestResidenceByName", itemName: itemName);

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
        }

        user.CheckCompletedQuests();

        var objectPopulation = gameItem.Population?.Min ?? -1;
        var worldPopulation = world.GetCurrentPopulation();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["retCoinYield"] = coinYield,
            ["retCashYield"] = cashYield,
            //response["doobers"] = AmfConverter.Convert(new List<int>());
            ["secureRands"] = AmfConverter.Convert(secureRands),
            ["objectPopulation"] = objectPopulation,
            ["worldPopulation"] = worldPopulation
        });
    }
}

public class HarvestRequest
{
    [AmfParam(1)] public BuildingHarvestRequest Building { get; set; } = new();
}

public class BuildingHarvestRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}