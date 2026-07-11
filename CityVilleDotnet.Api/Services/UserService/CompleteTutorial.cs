using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class CompleteTutorial(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Quests)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        player.CompleteTutorial();

        await context.SaveChangesAsync(cancellationToken);

        var quests = new ASObject()
        {
            { "QuestComponent", AmfConverter.Convert(player.Quests.Select(x => x.ToDto()).ToList()) }
        };

        return new CityVilleResponse().MetaData(quests);
    }
}