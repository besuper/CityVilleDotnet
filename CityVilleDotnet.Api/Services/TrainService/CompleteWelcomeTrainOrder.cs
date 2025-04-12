using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TrainService;

public class CompleteWelcomeTrainOrder(CityVilleDbContext context) : AmfService(context)
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        // TODO
        // amountFinal
        // orderCommodity (ex: goods)
        // orderAction (ex: buy)
        // timeSent (ex: 174545896)

        var trainInfo = _params[0] as ASObject;

        var user = await context.Set<User>()
            .Include(x => x.Quests)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.Commodities)
            .ThenInclude(x => x.Storage)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);

        var amount = (int)trainInfo["amountFinal"];

        user.AddGoods(amount);
        user.HandleQuestProgress("welcomeTrain");
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        var quests = new ASObject();
        quests["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active));

        var response = new CityVilleResponse(0, 333, quests);

        return response;
    }
}
