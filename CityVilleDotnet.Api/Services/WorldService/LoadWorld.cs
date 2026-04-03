using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public sealed class LoadWorld(CityVilleDbContext context, ILogger<LoadWorld> logger) : AmfService<LoadWorldRequest>
{
    public override async Task<ASObject> HandlePacket(LoadWorldRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var userToLoad = await context.Set<User>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.World)
            .ThenInclude(x => x!.MapRects)
            .FirstOrDefaultAsync(x => x.Player!.Snuid == request.TargetUsedId, cancellationToken);

        if (userToLoad?.Player is null)
            throw new Exception($"Unable to find user with Player.Uid {request.TargetUsedId}");

        if (userToLoad.GetPlayer().Id != playerId)
        {
            var currentUser = await context.Set<User>()
                .AsSplitQuery()
                .Include(x => x.Player)
                .ThenInclude(x => x.Quests)
                .Include(x => x.Player)
                .ThenInclude(x => x!.InventoryItems)
                .FirstOrDefaultAsync(x => x.Player.Id == playerId, cancellationToken);

            if (currentUser is null)
                throw new Exception("Current player not found");

            currentUser.GetPlayer().HandleQuestsProgress("neighborVisit");
            currentUser.GetPlayer().CheckCompletedQuests();

            await context.SaveChangesAsync(cancellationToken);
        }

        var dtoUser = userToLoad.ToDto();

        var response = (ASObject)AmfConverter.Convert(dtoUser.UserInfo);
        response!["franchises"] = new List<object>();

        return new CityVilleResponse().Data(response);
    }
}

public class LoadWorldRequest
{
    [AmfParam(0)] public int TargetUsedId { get; set; }
}