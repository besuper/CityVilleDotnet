using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class BuyUpgradeAction(CityVilleDbContext context) : AmfService<BuyUpgradeActionRequest>
{
    public override async Task<ASObject> HandlePacket(BuyUpgradeActionRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
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
        
        var requiredUpgradeActions = gameItem.Upgrade.GetRequiredUpgradeActions();

        var totalCost = requiredUpgradeActions - obj.UpgradeActionCount ?? 0;
        
        player.RemoveCash(totalCost);

        obj.SetUpgradeAction(requiredUpgradeActions);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class BuyUpgradeActionRequest
{
    [AmfParam(1)] public BuyUpgradeActionBuildingRequest Building { get; set; } = new();
}

public class BuyUpgradeActionBuildingRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}