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
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        if (@params.Length < 1)
            return GatewayService.CreateEmptyResponse();

        var questName = @params[0].ToString();

        if (questName is null)
            throw new Exception("Quest name can't be null");

        var user = await context.Set<User>()
            .Include(x => x.Quests)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null)
            throw new Exception($"User {userId} not found");

        if (user.Quests.Any(x => x.Name == questName))
            return new CityVilleResponse().Data(new ASObject { { "questStarted", 0 } });

        var quest = QuestSettingsManager.Instance.GetItem(questName);

        if (quest is null)
        {
            logger.LogError("Quest {QuestName} not found", questName);
            return new CityVilleResponse().Data(new ASObject { { "questStarted", 0 } });
        }

        var newQuest = Quest.Create(questName, quest.Tasks.Tasks.Count, QuestType.Active);
        user.Quests.Add(newQuest);

        await context.SaveChangesAsync(cancellationToken);

        logger.LogDebug("Started new quest {QuestName}", questName);

        var quests = new ASObject
        {
            { "QuestComponent", AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active).Select(x => x.ToDto())) }
        };

        return new CityVilleResponse().Data(new ASObject { { "questStarted", 1 } }).MetaData(quests);
    }
}