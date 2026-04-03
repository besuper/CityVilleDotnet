using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.VisitorService;

public sealed class RedeemVisitorHelpAction(CityVilleDbContext context) : AmfService<RedeemVisitorHelpActionRequest>
{
    public override async Task<ASObject> HandlePacket(RedeemVisitorHelpActionRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders.Where(o => o.SenderId == request.SenderId))
            .Include(x => x.Player)
            .ThenInclude(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.WorldObjectId))
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Masteries)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null) throw new Exception($"User not found with id {userId}");

        var visitOrder = user.Player.VisitorHelpOrders.FirstOrDefault(x => x.SenderId == request.SenderId && x.HelpTargets.Contains(request.WorldObjectId));

        if (visitOrder is null)
            throw new Exception("Can't find help visit order");

        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {request.ItemName}");

        if (request.Action == "harvest")
        {
            var world = user.GetPlayer().GetWorld();
            var obj = world.GetBuildingById(request.WorldObjectId) ?? throw new Exception($"Can't find building with id {request.WorldObjectId}");

            obj.Harvest();
            user.Player!.CollectDoobersRewards(obj.ContractName ?? obj.ItemName);

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
        }

        visitOrder.RemoveTarget(request.WorldObjectId);

        if (visitOrder.HelpTargets.Length == 0)
        {
            user.Player.VisitorHelpOrders.Remove(visitOrder);
            context.Remove(visitOrder);
        }

        await context.SaveChangesAsync(cancellationToken);

        // TODO: Implement response

        return new CityVilleResponse();
    }
}

public class RedeemVisitorHelpActionRequest
{
    [AmfParam(0)] public string SenderId { get; set; } = string.Empty;
    [AmfParam(1)] public int WorldObjectId { get; set; }
    [AmfParam(2)] public BuildingClassType ClassName { get; set; }
    [AmfParam(3)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(4)] public string Action { get; set; } = string.Empty;
}