using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class HandleQuestProgress(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // params
        // 0: action type (onValidCityName)

        var actionType = (string)@params[0];

        var user = await context.Set<User>()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .Include(x => x.World)
            .ThenInclude(x => x.Objects)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null) throw new Exception("Can't to find user with UserId");

        user.HandleQuestProgress(actionType);
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        var rep = new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active))
        };

        return new CityVilleResponse(0, 333, rep);
    }
}
