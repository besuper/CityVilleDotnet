using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class BuyUpgradeBuildings(CityVilleDbContext context) : AmfService<BuyUpgradeBuildingsRequest>
{
    public override async Task<ASObject> HandlePacket(BuyUpgradeBuildingsRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();
        var buildings = request.Params[0].Select(world.GetBuildingByClientId).ToList();

        var cashCost = GameSettingsManager.Instance.GetItem(buildings[0].ItemName)?.Upgrade?.CashCost;

        if (cashCost is null)
            return new CityVilleResponse().Error(GameErrorType.InvalidState);

        player.RemoveCash(Convert.ToInt32(cashCost));

        var results = new List<object>();

        foreach (var obj in buildings)
        {
            var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

            if (gameItem.Upgrade?.Name is null || gameItem.Upgrade.IsRandom)
                return new CityVilleResponse().Error(GameErrorType.InvalidState);

            player.HandleQuestsProgress("upgradeItemByName", itemName: obj.ItemName);

            obj.UpgradeBuilding(gameItem.GetFirstDeriveItem(gameItem), gameItem.Upgrade.Name);
            player.GiveUpgradeRewards(gameItem.Upgrade.Rewards?.Rewards ?? []);

            results.Add(new ASObject { ["id"] = obj.WorldFlatId });
        }

        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(results);
    }
}

public class BuyUpgradeBuildingsRequest
{
    [AmfParam(3)] public int[][] Params { get; set; } = [];
}

public class BuyUpgradeBuildingsRequestValidator : AbstractValidator<BuyUpgradeBuildingsRequest>
{
    public BuyUpgradeBuildingsRequestValidator()
    {
        RuleFor(x => x.Params).NotEmpty();
        RuleFor(x => x.Params[0]).NotEmpty().Must(ids => ids.Length <= 50).When(x => x.Params.Length > 0);
    }
}
