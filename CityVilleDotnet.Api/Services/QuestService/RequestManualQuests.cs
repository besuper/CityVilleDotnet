using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class RequestManualQuests(CityVilleDbContext context, ILogger<RequestManualQuests> logger) : AmfService<RequestManualQuestsRequest>
{
    public override async Task<ASObject> HandlePacket(RequestManualQuestsRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (request.Quests is null || request.Quests.Length == 0)
            return GatewayService.CreateEmptyResponse();

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Quests)
            .Include(x => x!.InventoryItems)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null)
            throw new Exception("Player not found");

        var results = new List<ASObject>();

        foreach (var questName in request.Quests)
        {
            var questItem = QuestSettingsManager.Instance.GetItem(questName);

            if (questItem is null)
            {
                logger.LogError("Can't find quest {QuestName} in RequestManualQuests", questName);
                continue;
            }

            if (player.Quests.Any(x => x.Name == questName)) continue;

            if (questItem.RequiredLevel is not null && player.Level < questItem.RequiredLevel) continue;
            if (questItem.RequiredPopulation is not null && player.GetWorld().Population < questItem.RequiredPopulation) continue;

            var newQuest = Quest.Create(questName, questItem.Tasks.Tasks.Count, QuestType.Active);
            player.Quests.Add(newQuest);

            logger.LogDebug("Starting quest {QuestName}", questName);

            player.GrantQuestStartItems(questItem);

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

public class RequestManualQuestsRequest
{
    [AmfParam(0)] public string[]? Quests { get; set; }
}