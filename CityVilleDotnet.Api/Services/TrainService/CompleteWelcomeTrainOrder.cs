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
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active).OrderBy(q => q.Order))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null)
            throw new Exception("Player not found");

        player.AddGoods(GameSettingsManager.Instance.GetSettings().WelcomeTrainQuestAmount);
        player.HandleQuestsProgress("welcomeTrain");

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}