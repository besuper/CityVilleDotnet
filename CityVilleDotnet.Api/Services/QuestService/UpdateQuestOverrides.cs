using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class UpdateQuestOverrides(CityVilleDbContext context, ILogger<UpdateQuestOverrides> logger) : AmfService<UpdateQuestOverridesRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateQuestOverridesRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.GlobalTableOverrides)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        foreach (var toAdd in request.OverridesToAdd)
        {
            if (!player.AddQuestTableOverride(toAdd.QuestName, toAdd.TaskId, toAdd.OverrideTable.Keyword, toAdd.OverrideTable.Table))
                logger.LogWarning("Rejected table {Table} for keyword {Keyword} from quest {QuestName} task {TaskId}", toAdd.OverrideTable.Table, toAdd.OverrideTable.Keyword, toAdd.QuestName, toAdd.TaskId);
        }

        foreach (var toRemove in request.OverridesToRemove)
        {
            player.RemoveQuestTableOverride(toRemove.QuestName, toRemove.OverrideTable.Keyword, toRemove.OverrideTable.Table);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class UpdateQuestOverridesRequest
{
    [AmfParam(0)] public QuestOverrideRequest[] OverridesToAdd { get; set; } = [];
    [AmfParam(1)] public QuestOverrideRequest[] OverridesToRemove { get; set; } = [];
}

public class QuestOverrideRequest
{
    [AmfParam("overrideTable")] public QuestOverrideTableRequest OverrideTable { get; set; } = new();
    [AmfParam("questName")] public string QuestName { get; set; } = string.Empty;
    [AmfParam("taskId")] public int TaskId { get; set; }
}

public class QuestOverrideTableRequest
{
    [AmfParam("table")] public string Table { get; set; } = string.Empty;
    [AmfParam("keyword")] public string Keyword { get; set; } = string.Empty;
}

public class UpdateQuestOverridesRequestValidator : AbstractValidator<UpdateQuestOverridesRequest>
{
    public UpdateQuestOverridesRequestValidator()
    {
        RuleForEach(x => x.OverridesToAdd).SetValidator(new QuestOverrideRequestValidator());
        RuleForEach(x => x.OverridesToRemove).SetValidator(new QuestOverrideRequestValidator());
    }
}

public class QuestOverrideRequestValidator : AbstractValidator<QuestOverrideRequest>
{
    public QuestOverrideRequestValidator()
    {
        RuleFor(x => x.QuestName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.TaskId).GreaterThanOrEqualTo(0);
        RuleFor(x => x.OverrideTable.Table).NotEmpty().MaximumLength(128);
        RuleFor(x => x.OverrideTable.Keyword).NotEmpty().MaximumLength(64);
    }
}
