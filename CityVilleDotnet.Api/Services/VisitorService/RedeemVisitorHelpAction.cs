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
    public override async Task<ASObject> HandlePacket(RedeemVisitorHelpActionRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.VisitorHelpOrders.Where(o => o.SenderId == request.SenderId))
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.WorldFlatId == request.WorldObjectId))
            .Include(x => x.InventoryItems)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Masteries)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var visitOrder = player.VisitorHelpOrders.FirstOrDefault(x => x.SenderId == request.SenderId && x.HelpTargets.Contains(request.WorldObjectId));

        if (visitOrder is null)
            throw new Exception("Can't find help visit order");

        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null)
            throw new Exception($"Can't find game item for {request.ItemName}");

        if (request.Action == "harvest")
        {
            var world = player.GetWorld();
            var obj = world.GetBuildingById(request.WorldObjectId) ?? throw new Exception($"Can't find building with id {request.WorldObjectId}");

            obj.Harvest();
            player.CollectDoobersRewards(obj.ContractName ?? obj.ItemName);

            player.HandleQuestsProgress("harvestByClass", className: obj.ClassName.ToString());

            if (obj.ClassName == BuildingClassType.Plot)
            {
                player.HandleQuestsProgress("harvestPlotByName", itemName: obj.ItemName);

                if (gameItem.HasMasteries())
                {
                    player.IncrementMastery(gameItem.Name);
                }
            }

            if (obj.ClassName == BuildingClassType.Business)
            {
                player.HandleQuestsProgress("harvestBusinessByName", itemName: obj.ItemName);
                player.HandleQuestsProgress("harvestBusinessByClass", className: obj.ClassName.ToString());
            }

            if (obj.ClassName == BuildingClassType.Residence)
            {
                player.HandleQuestsProgress("harvestResidenceByName", itemName: obj.ItemName);
            }

            player.CheckCompletedQuests();
        }

        visitOrder.RemoveTarget(request.WorldObjectId);

        if (visitOrder.HelpTargets.Length == 0)
        {
            player.VisitorHelpOrders.Remove(visitOrder);
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