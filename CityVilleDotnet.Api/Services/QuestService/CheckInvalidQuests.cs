using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.QuestService;

public class CheckInvalidQuests(CityVilleDbContext context, ILogger<CheckInvalidQuests> logger) : AmfService<CheckInvalidQuestsRequest>
{
    public override async Task<ASObject> HandlePacket(CheckInvalidQuestsRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (request.QuestNames.Length == 0)
            return GatewayService.CreateEmptyResponse();

        var player = await context.Set<Player>()
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        foreach (var questName in request.QuestNames.Where(x => !string.IsNullOrEmpty(x) && x.Length <= 64))
        {
            player.ExpireQuest(questName);
            logger.LogDebug("Quest {QuestName} expired for player {PlayerId}", questName, playerId);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class CheckInvalidQuestsRequest
{
    [AmfParam(0)] public string[] QuestNames { get; set; } = [];
}
