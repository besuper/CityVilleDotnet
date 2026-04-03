using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public sealed class UpdateSavedQuestOrder(CityVilleDbContext context) : AmfService<UpdateSavedQuestOrderRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateSavedQuestOrderRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        for (var i = 0; i < request.VisibleQuests.Length; i++)
        {
            var quest = player.Quests.FirstOrDefault(x => x.Name == request.VisibleQuests[i]);
            quest?.SetOrder(i, QuestLocation.Sidebar);
        }

        for (var i = 0; i < request.InMenuQuests.Length; i++)
        {
            var quest = player.Quests.FirstOrDefault(x => x.Name == request.InMenuQuests[i]);
            quest?.SetOrder(i, QuestLocation.Menu);
        }

        for (var i = 0; i < request.HiddenQuests.Length; i++)
        {
            var quest = player.Quests.FirstOrDefault(x => x.Name == request.HiddenQuests[i]);
            quest?.SetOrder(i, QuestLocation.Hidden);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class UpdateSavedQuestOrderRequest
{
    [AmfParam(0)] public string[] VisibleQuests { get; set; } = [];
    [AmfParam(1)] public string[] InMenuQuests { get; set; } = [];
    [AmfParam(2)] public string[] HiddenQuests { get; set; } = [];
    [AmfParam(3)] public string WorldType { get; set; } = string.Empty;
}