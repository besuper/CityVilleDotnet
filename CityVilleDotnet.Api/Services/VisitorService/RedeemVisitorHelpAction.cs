using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Pages;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.VisitorService;

public sealed class RedeemVisitorHelpAction(CityVilleDbContext context, ILogger<RedeemVisitorHelpAction> logger, IConfiguration configuration) : AmfService<RedeemVisitorHelpActionRequest>
{
    public override async Task<ASObject> HandlePacket(RedeemVisitorHelpActionRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var globalTableProviders = GameSettingsManager.Instance.GetGlobalTableProviders();
        var hubNames = GameSettingsManager.Instance.GetPreyGroups().SelectMany(x => x.GetHubNames()).ToList();
        var experiments = (configuration.GetSection("experiments").Get<List<Experiment>>() ?? []).ToDictionary(x => x.Name, x => x.Variant);

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.VisitorHelpOrders.Where(o => o.SenderId == request.SenderId))
            .Include(x => x.Worlds.Where(w => w.Type == WorldType.Main)) // TODO: Friends can only help main world?
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.WorldObjectId || globalTableProviders.Contains(o.ItemName) || hubNames.Contains(o.ItemName)))
            .Include(x => x.InventoryItems)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Masteries)
            .Include(x => x.GlobalTableOverrides)
            .Include(x => x.FeatureRollCounters)
            .Include(x => x.PreyGroups)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");
        
        var visitOrder = player.VisitorHelpOrders.FirstOrDefault(x => x.SenderId == request.SenderId && x.HelpTargets.Contains(request.WorldObjectId));

        if (visitOrder is null)
            throw new Exception("Can't find help visit order");

        var world = player.GetWorldByType(WorldType.Main) ?? throw new Exception("Main world not loaded");
        var obj = world.GetBuildingById(request.WorldObjectId) ?? throw new Exception($"Can't find building with id {request.WorldObjectId}");
        var action = obj.GetVisitReplayAction();

        if (action == "harvest")
        {

            if (obj.CanHarvest())
            {
                var itemName = obj.GetItemName();
                var className = obj.GetClassName();

                obj.Harvest();
                player.CollectDoobersRewards(itemName);
                player.RollPreySpawns(world, GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}"), experiments);

                player.HandleQuestsProgress("harvestByClass", className: className.ToString());
                player.HandleQuestsProgress("harvestResidenceByName", itemName: obj.ItemName);

                if (obj.ClassName == BuildingClassType.Plot)
                {
                    player.HandleQuestsProgress("harvestPlotByName", itemName: itemName);

                    if (GameSettingsManager.Instance.GetItem(itemName)?.HasMasteries() == true)
                    {
                        player.IncrementMastery(itemName);
                    }
                }

                if (obj.ClassName == BuildingClassType.Business)
                {
                    player.HandleQuestsProgress("harvestBusinessByName", itemName: itemName);
                    player.HandleQuestsProgress("harvestBusinessByClass", className: className.ToString());
                }
            }
        }

        if (action == "build")
        {

            if (obj.ClassName == BuildingClassType.ConstructionSite && !obj.IsConstructionComplete())
            {
                obj.AddConstructionStage();
            }
        }

        if (action == "")
        {

            if (obj.GetClassName() == BuildingClassType.Plot && obj.IsWithered())
            {
                obj.SetFullGrown();
            }
            else if (obj.GetClassName() == BuildingClassType.Plot && obj.State == WorldObjectState.Planted)
            {
                // Plot can be watered
                obj.BoostPlot();
            }
            else if (obj.GetClassName() == BuildingClassType.Business)
            {
                var businessItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

                player.CollectDoobersRewards(obj.ItemName);
                player.RollPreySpawns(world, businessItem, experiments);
                obj.CountBusinessUpgradeAction(businessItem, player.Level);
            }
            else
            {
                logger.LogError("Not supported help type {RequestAction}", action);
            }
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
}