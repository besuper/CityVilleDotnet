using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.MasteryGoalService;

public class BuyMasteryGoal(CityVilleDbContext context) : AmfService<BuyMasteryGoalRequest>
{
    public override async Task<ASObject> HandlePacket(BuyMasteryGoalRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Masteries)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var levels = GameSettingsManager.Instance.GetItem(request.MasteryName)?.MasteryItems.OrderBy(x => x.Level).ToList() ?? throw new Exception($"Can't find game item {request.MasteryName}");

        var mastery = player.Masteries.FirstOrDefault(x => x.ItemName == request.MasteryName);
        var count = mastery?.Count ?? 0;
        var currentLevel = mastery?.Level ?? 0;

        if (currentLevel != request.MasteryLevel || currentLevel + 1 >= levels.Count)
        {
            throw new Exception($"Can't buy mastery level {request.MasteryLevel} of {request.MasteryName}");
        }

        var current = levels[currentLevel];
        var nextRequired = levels[currentLevel + 1].RequiredCount ?? throw new Exception($"Missing mastery req on {request.MasteryName}");
        var remaining = nextRequired - count;
        var levelDiff = nextRequired - (current.RequiredCount ?? 0);

        player.RemoveCash(Math.Max((int)Math.Floor((double)remaining / levelDiff * (current.CashCost ?? 0)), 1));
        player.IncrementMastery(request.MasteryName, remaining);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class BuyMasteryGoalRequest
{
    [AmfParam(0)] public string MasteryName { get; set; } = string.Empty;
    [AmfParam(1)] public int MasteryLevel { get; set; }
}

public class BuyMasteryGoalValidator : AbstractValidator<BuyMasteryGoalRequest>
{
    public BuyMasteryGoalValidator()
    {
        RuleFor(x => x.MasteryName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.MasteryLevel).GreaterThanOrEqualTo(0);
    }
}