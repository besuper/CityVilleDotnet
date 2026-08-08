using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

internal sealed class PingFeedQuests(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Quests)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.CrewMembers)
            .Include(x => x.Masteries)
            .Include(x => x.InventoryItems)
            .Include(x => x.Franchises)
            .ThenInclude(x => x.Locations)
            .Include(x => x.SeenFlags)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        user.HandleQuestsProgress("");
        user.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().MetaData(new ASObject
        {
            ["QuestComponent"] = AmfConverter.Convert(user.Quests.ToQuestComponent())
        });
    }
}