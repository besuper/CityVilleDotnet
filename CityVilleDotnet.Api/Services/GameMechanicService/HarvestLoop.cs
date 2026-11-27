using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Api.Pages;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public sealed class HarvestLoop(CityVilleDbContext context, IConfiguration configuration) : AmfService<HarvestLoopRequest>
{
    public override async Task<ASObject> HandlePacket(HarvestLoopRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var globalTableProviders = GameSettingsManager.Instance.GetGlobalTableProviders();
        var hubNames = GameSettingsManager.Instance.GetPreyGroups().SelectMany(x => x.GetHubNames()).ToList();
        var experiments = (configuration.GetSection("experiments").Get<List<Experiment>>() ?? []).ToDictionary(x => x.Name, x => x.Variant);

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId || o.HarvestLoopStartTime != null || globalTableProviders.Contains(o.ItemName) || hubNames.Contains(o.ItemName) || o.EnergyModifier > 0))
            .Include(x => x.InventoryItems)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Coupons)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.GlobalTableOverrides)
            .Include(x => x.FeatureRollCounters)
            .Include(x => x.PreyGroups)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();
        var obj = world.GetBuildingByClientId(request.ObjectId);
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");
        var now = ServerUtils.GetCurrentUtcDateTime();

        if (request.Params.Operation == "hudTimerClicked")
        {
            player.BoostWonder(obj, gameItem, now);
        }
        else
        {
            player.HarvestWonder(world, obj, gameItem, now, request.Params.CouponName, experiments);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class HarvestLoopRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(3)] public HarvestLoopParamsRequest Params { get; set; } = new();
}

public class HarvestLoopParamsRequest
{
    [AmfParam("operation")] public string Operation { get; set; } = string.Empty;
    [AmfParam("couponName")] public string? CouponName { get; set; }
}

public class HarvestLoopRequestValidator : AbstractValidator<HarvestLoopRequest>
{
    public HarvestLoopRequestValidator()
    {
        RuleFor(x => x.Params.Operation).Must(x => x is "harvest" or "hudTimerClicked");
        RuleFor(x => x.Params.CouponName).MaximumLength(64);
    }
}
