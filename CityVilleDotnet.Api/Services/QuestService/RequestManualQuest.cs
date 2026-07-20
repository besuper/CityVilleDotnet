using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

internal sealed class RequestManualQuest(CityVilleDbContext context, ILogger<RequestManualQuest> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        if (@params.Length < 1)
            return GatewayService.CreateEmptyResponse();

        var questName = @params[0].ToString();

        if (questName is null)
            throw new Exception("Quest name can't be null");

        var player = await context.Set<Player>()
            .Include(x => x.Quests)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null)
            throw new Exception("Player not found");

        if (player.Quests.Any(x => x.Name == questName))
            return new CityVilleResponse().Data(new ASObject { { "questStarted", 0 } });

        var quest = QuestSettingsManager.Instance.GetItem(questName);

        if (quest is null)
        {
            logger.LogError("Quest {QuestName} not found", questName);
            return new CityVilleResponse().Data(new ASObject { { "questStarted", 0 } });
        }

        var newQuest = Quest.Create(questName, quest.Tasks.Tasks.Count, QuestType.Active);
        player.Quests.Add(newQuest);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogDebug("Started new quest {QuestName}", questName);

        var quests = new ASObject
        {
            { "QuestComponent", AmfConverter.Convert(player.Quests.ToQuestComponent()) }
        };

        return new CityVilleResponse().Data(new ASObject { { "questStarted", 1 } }).MetaData(quests);
    }
}