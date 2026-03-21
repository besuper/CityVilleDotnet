using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TrainService;

public class CompleteWelcomeTrainOrder(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active).OrderBy(q => q.Order))
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user?.Player is null)
            throw new Exception("Unable to find user with UserId");

        user.Player.AddGoods(GameSettingsManager.Instance.GetInt("WelcomeTrainQuestAmount"));
        user.HandleQuestsProgress("welcomeTrain");
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}