using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class RequestNightChange(CityVilleDbContext context) : AmfService<RequestNightChangeRequest>
{
    public override async Task<ASObject> HandlePacket(RequestNightChangeRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.SetNightMode(request.Enable);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class RequestNightChangeRequest
{
    [AmfParam(0)] public bool Enable { get; set; }
}
