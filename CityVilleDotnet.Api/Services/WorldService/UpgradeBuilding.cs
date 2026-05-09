using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
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
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.X == request.Building.Position.X && o.Y == request.Building.Position.Y))
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

        if (gameItem.Upgrade.CashCost is not null)
            player.RemoveCash(Convert.ToInt32(gameItem.Upgrade.CashCost));

        obj.UpgradeBuilding(gameItem.GetFirstDeriveItem(gameItem), newItemName);
        world.CalculatePopulation();

        var xpReward = gameItem.Upgrade.GetXpReward();

        if (xpReward > 0)
            player.AddXp(xpReward);

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