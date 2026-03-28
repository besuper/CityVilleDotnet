using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Clear(CityVilleDbContext context) : AmfService<ClearRequest>
{
    public override async Task<ASObject> HandlePacket(ClearRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't find user with UserId");

        if (user.Player is null) throw new Exception("Player not found for user");

        var world = user.GetWorld();

        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z) ?? throw new Exception($"Can't find building");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.EnergyCost?.Clear is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Clear);

            if (!user.Player!.RemoveEnergy(energyCost))
            {
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
            }
        }

        var secureRands = user.Player!.CollectDoobersRewards(obj.ItemName);

        // TODO: Implement remove franchise
        world.RemoveBuilding(obj);

        context.Set<WorldObject>().Remove(obj);

        user.HandleQuestsProgress("clearByClass", className: obj.ClassName.ToString()); // Wilderness
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["secureRands"] = AmfConverter.Convert(secureRands)
        });
    }
}

public class ClearRequest
{
    [AmfParam(1)] public BuildingClearRequest Building { get; set; } = new();
}

public class BuildingClearRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}