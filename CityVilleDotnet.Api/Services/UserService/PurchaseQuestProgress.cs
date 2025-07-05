using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class PurchaseQuestProgress(CityVilleDbContext context, ILogger<PurchaseQuestProgress> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Commodities)
            .ThenInclude(x => x!.Storage)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Can't to find user with UserId");

        var questName = (string)@params[0];
        var taskIndex = (int)@params[1];

        logger.LogInformation("Quest {QuestName} at {TaskIndex} is purchased", questName, taskIndex);

        var currentQuest = user.Quests.FirstOrDefault(x => x.Name == questName && x.QuestType == QuestType.Active);

        if (currentQuest is null)
            throw new Exception("Quest not found");

        // TODO: Check cashcost from task in QuestSettings
        currentQuest.PurchaseProgression(taskIndex);

        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}