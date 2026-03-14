using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

internal sealed class HarvestState(CityVilleDbContext context) : AmfService<HarvestStateRequest>
{
    public override async Task<ASObject> HandlePacket(HarvestStateRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null) throw new Exception($"User not found with id {userId}");

        var world = user.GetWorld();

        var obj = world.GetBuildingById(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.EnergyCost?.Harvest is not null)
        {
            var energyCost = int.Parse(gameItem.EnergyCost.Harvest);

            if (!user.Player.RemoveEnergy(energyCost))
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);
        }

        obj.Harvest();
        user.Player.CollectDoobersRewards(obj.ContractName ?? obj.ItemName, obj.ClassName);

        obj.Close();

        user.HandleQuestsProgress("harvestByClass", className: obj.ClassName.ToString());
        user.HandleQuestsProgress("harvestBusinessByName", itemName: obj.ItemName);
        user.HandleQuestsProgress("harvestBusinessByClass", className: obj.ClassName.ToString());
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class HarvestStateRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
}
