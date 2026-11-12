using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class BuyAllUpgradeActions(CityVilleDbContext context) : AmfService<BuyAllUpgradeActionsRequest>
{
    public override async Task<ASObject> HandlePacket(BuyAllUpgradeActionsRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(o => o.X == request.Building.Position.X && o.Y == request.Building.Position.Y))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception("Can't find building");
        var upgrade = GameSettingsManager.Instance.GetItem(obj.ItemName)?.Upgrade ?? throw new Exception($"Can't find upgrade for {obj.ItemName}");
        var requiredUpgradeActions = upgrade.GetRequiredUpgradeActions();

        // UpgradesUtil::calculateCashCostAllUpgradeActions, businessUpgradeCost is 1
        player.RemoveCash(Math.Max(requiredUpgradeActions - (obj.UpgradeActionCount ?? 0), 0));
        obj.SetUpgradeAction(requiredUpgradeActions);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class BuyAllUpgradeActionsRequest
{
    [AmfParam(1)] public BuyAllUpgradeActionsBuildingRequest Building { get; set; } = new();
}

public class BuyAllUpgradeActionsBuildingRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}
