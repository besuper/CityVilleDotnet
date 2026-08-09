using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TrainService;

public class AcceptTrain(CityVilleDbContext context, ILogger<AcceptTrain> logger) : AmfService<AcceptTrainRequest>
{
    private const string GoldenTrainStatue = "deco_goldentrain_statue";
    private const string PayoutBonusUnlockFlag = "cux_train_bonus";

    public override async Task<ASObject> HandlePacket(AcceptTrainRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.TrainOrder)
            .ThenInclude(x => x!.Workers)
            .Include(x => x.SeenFlags)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();
        var order = world.TrainOrder ?? throw new Exception("No train order in progress");

        if (order.ItemName != request.ItemName)
            throw new Exception($"Train order mismatch, {order.ItemName} is on its way but {request.ItemName} was accepted");

        if (!order.HasArrived(ServerUtils.GetCurrentTimeSeconds()))
            throw new Exception($"Train {order.ItemName} has not arrived yet");

        var hasStatue = await context.Set<World>()
            .AnyAsync(x => x.Id == world.Id && x.Objects.Any(o => o.ItemName == GoldenTrainStatue), cancellationToken);

        var payout = order.GetPayout(hasStatue, player.HasSeenFlag(PayoutBonusUnlockFlag));
        
        logger.LogDebug("Train payout {Payout}", payout);

        if (order.Operation == TrainOperationType.Sell)
            player.AddCoins(payout);
        else
            player.AddGoods(payout);

        player.HandleQuestsProgress("acceptTrain");
        player.HandleQuestsProgress("acceptTrainByName", itemName: order.ItemName);

        world.ClearTrainOrder();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class AcceptTrainRequest
{
    [AmfParam(0)] public string Bucket { get; set; } = string.Empty;
    [AmfParam(1)] public string ItemName { get; set; } = string.Empty;
}

public class AcceptTrainValidator : AbstractValidator<AcceptTrainRequest>
{
    public AcceptTrainValidator()
    {
        RuleFor(x => x.Bucket).NotEmpty().MaximumLength(16);
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}
