using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class UpgradeBuilding(CityVilleDbContext context) : AmfService<UpgradeBuildingRequest>
{
    public override async Task<ASObject> HandlePacket(UpgradeBuildingRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (user.Player is null) throw new Exception($"User not found with id {userId}");

        var world = user.GetPlayer().GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception("Can't find building");
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.Upgrade is null)
            throw new Exception("The game item doesn't contains upgrade item");

        // TODO: Check if crew members are purchased
        // TODO: Check requirements (population, ...)

        var newItemName = gameItem.Upgrade.Name;

        if (gameItem.Upgrade.CashCost is not null)
        {
            user.Player?.RemoveCash(Convert.ToInt32(gameItem.Upgrade.CashCost));
        }

        obj.UpgradeBuilding(newItemName);
        world.CalculatePopulation();

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