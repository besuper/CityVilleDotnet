using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TrainService;

public class CompleteWelcomeTrainOrder(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] _params, Guid userId, CancellationToken cancellationToken)
    {
        // TODO
        // amountFinal
        // orderCommodity (ex: goods)
        // orderAction (ex: buy)
        // timeSent (ex: 174545896)

        var trainInfo = _params[0] as ASObject ?? throw new Exception("trainInfo is null");

        var user = await context.Set<User>()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x.Commodities)
            .ThenInclude(x => x.Storage)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null)
            throw new Exception("Unable to find user with UserId");

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
