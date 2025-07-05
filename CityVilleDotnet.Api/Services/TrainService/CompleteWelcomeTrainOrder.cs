using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TrainService;

public class CompleteWelcomeTrainOrder(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        // TODO
        // amountFinal
        // orderCommodity (ex: goods)
        // orderAction (ex: buy)
        // timeSent (ex: 174545896)

        var trainInfo = @params[0] as ASObject ?? throw new Exception("trainInfo is null");

        var user = await context.Set<User>()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Commodities)
            .ThenInclude(x => x!.Storage)
            // FIXME: This should not be here
            // Trigger task countWorldObjectByName
            // Might be fixed with quests rework
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null)
            throw new Exception("Unable to find user with UserId");

        var amount = (int)trainInfo["amountFinal"];

        user.AddGoods(amount);
        user.HandleQuestProgress("welcomeTrain");
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        var quests = new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active))
        };

        var response = new CityVilleResponse(0, 333, quests);

        return response;
    }
}