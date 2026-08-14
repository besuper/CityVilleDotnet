using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class UpgradeBuilding(CityVilleDbContext context) : AmfService<UpgradeBuildingRequest>
{
    public override async Task<ASObject> HandlePacket(UpgradeBuildingRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.SeenFlags)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception("Can't find building");
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.Upgrade is null)
            throw new Exception("The game item doesn't contains upgrade item");

        // TODO: Check if crew members are purchased
        // TODO: Check requirements (population, ...)

        var requiredLevel = gameItem.Upgrade.GetRequiredLevel();

        if (requiredLevel > 0 && player.Level < requiredLevel)
            return new CityVilleResponse().Error(GameErrorType.InvalidState);

        var requiredUpgradeActions = gameItem.Upgrade.GetRequiredUpgradeActions();

        if (requiredUpgradeActions > 0 && (obj.UpgradeActionCount ?? 0) < requiredUpgradeActions)
            return new CityVilleResponse().Error(GameErrorType.InvalidState);

        var newItemName = gameItem.Upgrade.Name;
        
        // By default, upgrade mechanic has a gate linked with a gateName; sometimes not and use pre_upgrade as fallback
        var upgradeGateName = gameItem.GetGameEventMechanic("upgrade")?.GateName ?? (obj.ClassName == BuildingClassType.Municipal ? "pre_upgrade" : null);
        var gateKeyCount = upgradeGateName is null ? 0 : gameItem.GetInventoryGateKeys(upgradeGateName).Count;

        // No inventory gate, just remove the cash
        if (gateKeyCount == 0 && gameItem.Upgrade.CashCost is not null)
            player.RemoveCash(Convert.ToInt32(gameItem.Upgrade.CashCost));

        // If the item has an inventory gate, we remove the items
        if (upgradeGateName is not null)
        {
            foreach (var consumed in player.ConsumeInventoryGate(gameItem, upgradeGateName))
                context.Set<InventoryItem>().Remove(consumed);
        }

        player.HandleQuestsProgress("upgradeItemByName", itemName: obj.ItemName);

        obj.UpgradeBuilding(gameItem.GetFirstDeriveItem(gameItem), newItemName);
        world.CalculatePopulation();

        player.GiveUpgradeRewards(gameItem.Upgrade?.Rewards?.Rewards ?? []);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class UpgradeBuildingRequest
{
    [AmfParam(1)] public BuildingUpgradeBuildingRequest Building { get; set; } = new();
}

public class BuildingUpgradeBuildingRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}