using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class LoadWorld(CityVilleDbContext context, ILogger<LoadWorld> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var visitUserId = (string)@params[0];

        logger.LogInformation("LoadWorld for user {UserId} visiting {VisitUserId}", userId, visitUserId);

        var userToLoad = await context.Set<User>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.Player!.Uid.ToString() == visitUserId, cancellationToken);

        if (userToLoad is null)
            throw new Exception($"Unable to find user with Player.Uid {visitUserId}");

        if (userToLoad.UserId.ToString() != userId.ToString())
        {
            var currentUser = await context.Set<User>()
                .AsSplitQuery()
                .Include(x => x.Quests)
                .Include(x => x.Player)
                .ThenInclude(x => x!.InventoryItems)
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (currentUser is null)
                throw new Exception($"Unable to find current user with UserId {userId}");

            currentUser.HandleQuestsProgress("neighborVisit");
            currentUser.CheckCompletedQuests();

            await context.SaveChangesAsync(cancellationToken);
        }

        var dtoUser = userToLoad.ToDto();

        var response = (ASObject)AmfConverter.Convert(dtoUser.UserInfo);
        response!["franchises"] = new List<object>();

        return new CityVilleResponse().Data(response);
    }
}