using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class HandleQuestProgress(CityVilleDbContext context) : AmfService(context)
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        // params
        // 0: action type (onValidCityName)

        var actionType = (string)_params[0];

        var user = await context.Set<User>()
            .Include(x => x.Quests)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        foreach (var quest in user.Quests)
        {
            var questItem = QuestSettingsManager.Instance.GetItem(quest.Name);

            if (questItem is null) continue;

            var index = 0;

            foreach (var task in questItem.Tasks.Tasks)
            {
                if (task.Action.Equals(actionType))
                {
                    quest.Progress[index] = 1;
                }

                index++;
            }
        }

        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        var rep = new ASObject();
        rep["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active));

        return new CityVilleResponse(0, 333, rep);
    }
}
