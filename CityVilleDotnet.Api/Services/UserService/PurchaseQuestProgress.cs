using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class PurchaseQuestProgress(CityVilleDbContext context, ILogger<PurchaseQuestProgress> logger) : AmfService<PurchaseQuestProgressRequest>
{
    public override async Task<ASObject> HandlePacket(PurchaseQuestProgressRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Quests)
            .Include(x => x.SeenFlags)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        logger.LogDebug("Quest {QuestName} at {TaskIndex} is purchased", request.QuestName, request.TaskIndex);

        var currentQuest = player.Quests.FirstOrDefault(x => x.Name == request.QuestName && x.QuestType == QuestType.Active);

        if (currentQuest is null) throw new Exception("Quest not found");

        var questItem = QuestSettingsManager.Instance.GetItem(request.QuestName);

        if (questItem is null) throw new Exception("Quest settings not found");

        var task = questItem.Tasks.Tasks[request.TaskIndex];
        var cashCost = task.CashCost ?? 0;

        if (cashCost > 0)
        {
            if (player.Cash < cashCost)
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

            player.RemoveCash(cashCost);
        }

        currentQuest.PurchaseProgression(request.TaskIndex);

        player.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().MetaData(new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(player.Quests.Select(x => x.ToDto()))
        });
    }
}

public class PurchaseQuestProgressRequest
{
    [AmfParam(0)] public string QuestName { get; set; } = string.Empty;
    [AmfParam(1)] public int TaskIndex { get; set; }
}

public class PurchaseQuestProgressValidator : AbstractValidator<PurchaseQuestProgressRequest>
{
    public PurchaseQuestProgressValidator()
    {
        RuleFor(x => x.QuestName).NotEmpty().MaximumLength(64);
    }
}