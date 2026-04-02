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
    public override async Task<ASObject> HandlePacket(PurchaseQuestProgressRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't to find user with UserId");

        logger.LogDebug("Quest {QuestName} at {TaskIndex} is purchased", request.QuestName, request.TaskIndex);

        var currentQuest = user.Quests.FirstOrDefault(x => x.Name == request.QuestName && x.QuestType == QuestType.Active);

        if (currentQuest is null) throw new Exception("Quest not found");

        var questItem = QuestSettingsManager.Instance.GetItem(request.QuestName);

        if (questItem is null) throw new Exception("Quest settings not found");

        var task = questItem.Tasks.Tasks[request.TaskIndex];
        var cashCost = task.CashCost ?? 0;

        if (cashCost > 0)
        {
            var player = user.GetPlayer();

            if (player.Cash < cashCost)
                return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

            player.RemoveCash(cashCost);
        }

        currentQuest.PurchaseProgression(request.TaskIndex);

        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().MetaData(new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.Select(x => x.ToDto()))
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