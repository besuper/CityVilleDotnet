using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Pages;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.AutomationService;

public class AutoCollectRent(CityVilleDbContext context, IConfiguration configuration) : AmfService<AutoCollectRentRequest>
{
    public override async Task<ASObject> HandlePacket(AutoCollectRentRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var globalTableProviders = GameSettingsManager.Instance.GetGlobalTableProviders();
        var hubNames = GameSettingsManager.Instance.GetPreyGroups().SelectMany(x => x.GetHubNames()).ToList();
        var experiments = (configuration.GetSection("experiments").Get<List<Experiment>>() ?? []).ToDictionary(x => x.Name, x => x.Variant);

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => request.MapResourceIds.Contains(o.WorldFlatId) || request.MapResourceIds.Contains(o.TempId) || globalTableProviders.Contains(o.ItemName) || hubNames.Contains(o.ItemName) || o.ClassName == BuildingClassType.DockHouse))
            .ThenInclude(x => x.Workers)
            .Include(x => x.InventoryItems)
            .Include(x => x.SeenFlags)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Masteries)
            .Include(x => x.GlobalTableOverrides)
            .Include(x => x.FeatureRollCounters)
            .Include(x => x.PreyGroups)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();
        var objects = request.MapResourceIds.Select(world.GetBuildingByClientId).ToList();
        var actionTime = ServerUtils.GetActionTime(request.ClientEnqueueTime);

        player.PayAutomation(objects, ServerUtils.GetCurrentTimeSeconds());
        player.HandleQuestsProgress("autoHarvest", amount: objects.Count);

        foreach (var obj in objects)
        {
            player.HandleQuestsProgress("harvestMechanicByKeywords", itemName: obj.ItemName);
        }

        foreach (var obj in objects.Where(o => o.CanHarvest()))
        {
            var itemName = obj.GetItemName();
            var contractName = obj.ContractName;

            player.HarvestObject(world, obj, actionTime);
            player.TrackHarvest(world, obj, itemName, contractName, experiments);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class AutoCollectRentRequest
{
    [AmfParam(0)] public int[] MapResourceIds { get; set; } = [];
    [AmfParam(1)] public long? ClientEnqueueTime { get; set; }
}

public class AutoCollectRentRequestValidator : AbstractValidator<AutoCollectRentRequest>
{
    public AutoCollectRentRequestValidator()
    {
        RuleFor(x => x.MapResourceIds).NotEmpty().Must(ids => ids.Distinct().Count() == ids.Length);
    }
}
