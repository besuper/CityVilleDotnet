using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Harvest(CityVilleDbContext context, ILogger<HarvestRequest> logger) : AmfService<HarvestRequest>
{
    public override async Task<ASObject> HandlePacket(HarvestRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Masteries)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (user.Player is null) throw new Exception("Player not found for user");

        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception("Can't find building");

        var itemName = obj.ClassName == BuildingClassType.Plot ? obj.ContractName : obj.ItemName;

        if (itemName is null)
            throw new Exception("Item name is null, can't harvest");

        var gameItem = GameSettingsManager.Instance.GetItem(itemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {itemName}");

        if (gameItem.EnergyCost?.Harvest is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Harvest);

            if (!user.Player!.RemoveEnergy(energyCost))
            {
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
            }
        }

        var (coinYield, cashYield) = obj.Harvest();
        var secureRands = user.Player!.CollectDoobersRewards(obj.ContractName ?? obj.ItemName, obj.ClassName);

        logger.LogDebug("Secure rands {Join}", string.Join(",", secureRands.ToArray()));
        logger.LogDebug("Secure rands {SecureRandsCount}", secureRands.Count);

        user.HandleQuestsProgress("harvestByClass", className: obj.ClassName.ToString());

        if (obj.ClassName == BuildingClassType.Plot)
        {
            user.HandleQuestsProgress("harvestPlotByName", itemName: obj.ItemName);

            if (gameItem.HasMasteries())
            {
                user.Player.IncrementMastery(gameItem.Name);
            }
        }

        if (obj.ClassName == BuildingClassType.Business)
        {
            user.HandleQuestsProgress("harvestBusinessByName", itemName: obj.ItemName);
            user.HandleQuestsProgress("harvestBusinessByClass", className: obj.ClassName.ToString());
        }

        if (obj.ClassName == BuildingClassType.Residence)
        {
            user.HandleQuestsProgress("harvestResidenceByName", itemName: obj.ItemName);
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
        }).MetaData(new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active).Select(x => x.ToDto()))
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