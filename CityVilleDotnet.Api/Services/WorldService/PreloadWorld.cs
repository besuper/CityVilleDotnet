using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class PreloadWorld(CityVilleDbContext context, ILogger<LoadWorld> logger) : AmfService<PreloadWorldRequest>
{
    public override async Task<ASObject> HandlePacket(PreloadWorldRequest request, Guid userId, CancellationToken cancellationToken)
    {
        logger.LogInformation("LoadWorld for user {UserId} visiting {VisitUserId}", userId, request.VisitUserId);

        var user = await context.Set<User>()
            .AsSplitQuery()
            .AsNoTracking()
            .Include(x => x.Quests)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Player)
            .ThenInclude(x => x!.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .ThenInclude(x => x.World)
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
            .FirstOrDefaultAsync(x => x.Player!.Snuid == request.VisitUserId, cancellationToken);

        if (user is null)
            throw new Exception($"Unable to find user with Player.Uid {request.VisitUserId}");

        var dtoUser = user.ToDto();

        var response = (ASObject)AmfConverter.Convert(dtoUser.UserInfo);
        response!["franchises"] = new List<object>();

        return new CityVilleResponse().Data(response);
    }
}

public sealed class PreloadWorldRequest
{
    [AmfParam(0)] public int VisitUserId { get; set; }
}