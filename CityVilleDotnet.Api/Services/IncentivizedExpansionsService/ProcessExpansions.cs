using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.IncentivizedExpansionsService;

public class ProcessExpansions(CityVilleDbContext context, ILogger<ProcessExpansions> logger) : AmfService<ProcessExpansionsRequest>
{
    public override async Task<ASObject> HandlePacket(ProcessExpansionsRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.World)
            .ThenInclude(x => x!.IncentivizedExpansions)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't find player");

        var world = player.GetWorld();
        var results = new List<object>();

        foreach (var operation in request.Operations)
        {
            var expansionId = operation.Params.IncExpId;
            var config = GameSettingsManager.Instance.GetDynamicExpansion(expansionId);

            if (config is null)
            {
                logger.LogWarning("Unknown dynamic expansion {ExpansionId}", expansionId);
                continue;
            }

            switch (operation.Op)
            {
                case "storeData":
                {
                    if (operation.Params.X is null || operation.Params.Y is null)
                        throw new Exception($"Missing coordinates for storeData on {expansionId}");

                    world.GetOrCreateIncentivizedExpansion(expansionId).Store(operation.Params.X.Value, operation.Params.Y.Value, ServerUtils.GetCurrentTime());
                    break;
                }
                case "recordFailure":
                {
                    var rootId = config.GetFallbackChainRoot().Id;

                    world.GetOrCreateIncentivizedExpansion(rootId).IncrementFailureCount();
                    break;
                }
                case "grantRewards":
                {
                    // If placement fail => send to inventory
                    foreach (var reward in config.GetRewards().Where(x => x.Callback == "embedRewardInWorld"))
                    {
                        player.AddItem(reward.ItemName);
                    }

                    world.GetOrCreateIncentivizedExpansion(expansionId).Complete();
                    break;
                }
                case "persistExpansionInWorld":
                {
                    if (operation.Params.X is null || operation.Params.Y is null)
                        throw new Exception($"Missing coordinates for persistExpansionInWorld on {expansionId}");

                    var tempIds = operation.Params.TempIds ?? [];
                    var teasers = config.GetTeasers()
                        .Where(x => GameSettingsManager.Instance.GetItem(x.ItemName)?.InteractOnLock == true)
                        .ToList();

                    if (teasers.Count != tempIds.Length)
                        logger.LogWarning("Teasers count mismatch for {ExpansionId}: {TeasersCount} interactOnLock teasers, {TempIdsCount} tempIds", expansionId, teasers.Count, tempIds.Length);

                    var finalIds = new List<int>();

                    foreach (var (teaser, tempId) in teasers.Zip(tempIds))
                    {
                        var obj = world.EmbedDynamicExpansionObject(teaser, operation.Params.X.Value, operation.Params.Y.Value, tempId);

                        finalIds.Add(obj.WorldFlatId);
                    }

                    results.Add(new ASObject
                    {
                        { "incExpId", expansionId },
                        { "finalIds", finalIds },
                    });
                    break;
                }
                case "grantFreeExpansion":
                {
                    var expansion = world.IncentivizedExpansions.FirstOrDefault(x => x.ExpansionId == expansionId);

                    if (expansion is null || !expansion.IsActive())
                        throw new Exception($"Can't grant free expansion, {expansionId} is not active");

                    if (config.GrantFreeExpansionType is null)
                        throw new Exception($"Dynamic expansion {expansionId} has no grantFreeExpansionType");

                    var expandItem = GameSettingsManager.Instance.GetItem(config.GrantFreeExpansionType);

                    if (expandItem?.Height is null || expandItem.Width is null)
                        throw new Exception($"Can't find expansion item {config.GrantFreeExpansionType}");

                    var x = expansion.X!.Value;
                    var y = expansion.Y!.Value;

                    if (!world.MapRects.Any(m => m.X == x && m.Y == y))
                    {
                        world.AddMapRect(new MapRect
                        {
                            X = x,
                            Y = y,
                            Width = expandItem.Width.Value,
                            Height = expandItem.Height.Value
                        });
                    }

                    var treesInfo = operation.Params.TreeAndTempIdInfo?.TreesInfo ?? [];

                    foreach (var tree in treesInfo.SelectMany(t => t))
                    {
                        if (GameSettingsManager.Instance.GetItem(tree.ItemName) is null)
                            throw new Exception($"Can't find tree item {tree.ItemName}");

                        var newTree = new WorldObject(
                            tree.ItemName,
                            BuildingClassType.Wilderness,
                            null,
                            false,
                            tree.Id,
                            WorldObjectState.Static,
                            tree.Dir,
                            ServerUtils.GetCurrentTime(),
                            ServerUtils.GetCurrentTime(),
                            tree.X,
                            tree.Y,
                            0,
                            world.GetAvailableBuildingId()
                        );

                        world.AddBuilding(newTree);
                    }

                    var rewardTempIds = (operation.Params.TreeAndTempIdInfo?.RewardTempIds ?? []).SelectMany(r => r);
                    var rewards = config.GetRewards().Where(r => r.Callback == "embedRewardInWorld");

                    foreach (var (reward, tempId) in rewards.Zip(rewardTempIds))
                    {
                        world.EmbedDynamicExpansionObject(reward, x, y, tempId);
                    }

                    expansion.Complete();
                    break;
                }
                default:
                    logger.LogWarning("Unknown processExpansions operation {Op}", operation.Op);
                    break;
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(results);
    }
}

public class ProcessExpansionsRequest
{
    [AmfParam(0)] public ProcessExpansionsOperation[] Operations { get; set; } = [];
}

public class ProcessExpansionsOperation
{
    [AmfParam("op")] public string Op { get; set; } = string.Empty;
    [AmfParam("params")] public ProcessExpansionsParams Params { get; set; } = new();
}

public class ProcessExpansionsParams
{
    [AmfParam("incExpId")] public string IncExpId { get; set; } = string.Empty;
    [AmfParam("x")] public int? X { get; set; }
    [AmfParam("y")] public int? Y { get; set; }
    [AmfParam("tempIds")] public int[]? TempIds { get; set; }
    [AmfParam("treeAndTempIdInfo")] public TreeAndTempIdInfoParam? TreeAndTempIdInfo { get; set; }
}

public class TreeAndTempIdInfoParam
{
    [AmfParam("rewardTempIds")] public int[][] RewardTempIds { get; set; } = [];
    [AmfParam("treesInfo")] public ExpansionTreeInfoParam[][] TreesInfo { get; set; } = [];
}

public class ExpansionTreeInfoParam
{
    [AmfParam("id")] public int Id { get; set; }
    [AmfParam("itemName")] public string ItemName { get; set; } = string.Empty;
    [AmfParam("x")] public int X { get; set; }
    [AmfParam("y")] public int Y { get; set; }
    [AmfParam("dir")] public int Dir { get; set; }
}

public class ProcessExpansionsRequestValidator : AbstractValidator<ProcessExpansionsRequest>
{
    public ProcessExpansionsRequestValidator()
    {
        RuleForEach(x => x.Operations).ChildRules(operation =>
        {
            operation.RuleFor(x => x.Op).NotEmpty().MaximumLength(64);
            operation.RuleFor(x => x.Params.IncExpId).NotEmpty().MaximumLength(64);
        });
    }
}
