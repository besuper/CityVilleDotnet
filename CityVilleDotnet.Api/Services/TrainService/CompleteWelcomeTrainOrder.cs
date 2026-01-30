using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TrainService;

public class CompleteWelcomeTrainOrder(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            // FIXME: This should not be here
            // Trigger task countWorldObjectByName
            // Might be fixed with quests rework
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null)
            throw new Exception("Unable to find user with UserId");

        // FIXME: Don't use hard coded value (use welcomeTrainQuestAmount from game settings)
        var amount = Convert.ToInt32(250);

        user.Player.AddGoods(amount);
        user.HandleQuestsProgress("welcomeTrain");
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        var quests = new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.Where(x => x.QuestType == QuestType.Active).Select(x => x.ToDto()))
        };

        return new CityVilleResponse().MetaData(quests);
    }
}