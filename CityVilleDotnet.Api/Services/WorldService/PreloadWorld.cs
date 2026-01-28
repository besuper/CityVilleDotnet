using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class PreloadWorld(CityVilleDbContext context, ILogger<LoadWorld> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var visitUserId = (string)@params[0];

        logger.LogInformation("LoadWorld for user {UserId} visiting {VisitUserId}", userId, visitUserId);

        var user = await context.Set<User>()
            .AsSplitQuery()
            .AsNoTracking()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Licenses)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Franchises)
            .ThenInclude(x => x.Locations)
            .Include(x => x.Player)
            .ThenInclude(x => x!.LotOrders) // FIXME: Limit orders
            .Include(x => x.Player)
            .ThenInclude(x => x!.VisitorHelpOrders) // FIXME: Limit orders
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null)
            throw new Exception($"Unable to find user with Player.Uid {visitUserId}");

        var dtoUser = user.ToDto();

        var response = (ASObject)AmfConverter.Convert(dtoUser.UserInfo);
        response!["franchises"] = new List<object>();

        return new CityVilleResponse().Data(response);
    }
}