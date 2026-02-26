using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class RequestManualQuests(CityVilleDbContext context, ILogger<RequestManualQuests> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        if (@params.Length < 1)
            return GatewayService.CreateEmptyResponse();

        var quests = @params[0] as object[];

        if (quests is null)
            return GatewayService.CreateEmptyResponse();

        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null)
            throw new Exception($"User {userId} not found");

        var results = new List<ASObject>();

        foreach (string questName in quests)
        {
            var questItem = QuestSettingsManager.Instance.GetItem(questName);

            if (questItem is null)
            {
                logger.LogError("Can't find quest {QuestName} in RequestManualQuests", questName);
                results.Add(new ASObject
                {
                    ["errorType"] = 0,
                    ["questName"] = questName,
                    ["questStarted"] = false
                });
                continue;
            }

            if (user.Quests.Any(x => x.Name == questName))
            {
                results.Add(new ASObject
                {
                    ["errorType"] = 0,
                    ["questName"] = questName,
                    ["questStarted"] = false
                });
                continue;
            }

            var newQuest = Quest.Create(questName, questItem.Tasks.Tasks.Count, QuestType.Active);
            user.Quests.Add(newQuest);

            logger.LogDebug("Starting quest {QuestName}", questName);

            var itemsToGive = QuestSettingsManager.QuestStartInventoryItem.GetValueOrDefault(questName);

            if (itemsToGive is not null)
            {
                foreach (var item in itemsToGive)
                {
                    user.Player.AddItem(item);
                }
            }

            results.Add(new ASObject
            {
                ["errorType"] = 0,
                ["questName"] = questName,
                ["questStarted"] = true
            });
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(results);
    }
}