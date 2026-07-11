using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.GameWorlds;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class PreloadWorld(CityVilleDbContext context, ILogger<LoadWorld> logger) : AmfService<PreloadWorldRequest>
{
    public override async Task<ASObject> HandlePacket(PreloadWorldRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        logger.LogInformation("PreloadWorld for user {UserId} visiting {VisitUserId} world {WorldType}", playerId, request.VisitUserId, request.Type);

        await DowntownWorldFactory.EnsureCreatedAsync(context, playerId, request.VisitUserId, request.Type, cancellationToken);

        var user = await context.Set<Player>()
            .AsSplitQuery()
            .AsNoTracking()
            .Include(x => x.Quests)
            .Include(x => x.InventoryItems)
            .Include(x => x.Worlds.Where(w => w.Type == request.Type))
            .ThenInclude(w => w.MapRects)
            .Include(x => x.Worlds.Where(w => w.Type == request.Type))
            .ThenInclude(w => w.Objects)
            .ThenInclude(o => o.MechanicCounters)
            .Include(x => x.Worlds.Where(w => w.Type == request.Type))
            .ThenInclude(w => w.Objects)
            .ThenInclude(o => o.StorageItems)
            .Include(x => x.Worlds.Where(w => w.Type == request.Type))
            .ThenInclude(w => w.Objects)
            .ThenInclude(o => o.Slots)
            .Include(x => x.Worlds.Where(w => w.Type == request.Type))
            .ThenInclude(w => w.IncentivizedExpansions)
            .Include(x => x.SeenFlags)
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendPlayer)
            .ThenInclude(x => x.Worlds.Where(w => w.Type == WorldType.Main))
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Licenses)
            .Include(x => x.Franchises)
            .ThenInclude(x => x.Locations)
            .Include(x => x.LotOrders) // FIXME: Limit orders
            .Include(x => x.VisitorHelpOrders) // FIXME: Limit orders
            .FirstOrDefaultAsync(x => x.Snuid == request.VisitUserId, cancellationToken);

        if (user is null)
            throw new Exception($"Unable to find user with Player.Uid {request.VisitUserId}");

        // In-memory only (AsNoTracking) so the DTO targets the requested world
        user.SwitchWorld(request.Type);

        logger.LogInformation("PreloadWorld sending world {WorldType} with population {Population}/{PopulationCap}",
            user.GetWorld().Type, user.GetWorld().Population, user.GetWorld().PopulationCap);

        // Rows only, the summaries of the non active worlds are built from the persisted fields
        var allWorlds = await context.Set<World>()
            .AsNoTracking()
            .Where(w => w.Player!.Id == user.Id)
            .ToListAsync(cancellationToken);

        var dtoUser = user.ToDto(allWorlds);

        var response = (ASObject)AmfConverter.Convert(dtoUser.UserInfo);
        response!["franchises"] = new List<object>();

        return new CityVilleResponse().Data(response);
    }
}

public sealed class PreloadWorldRequest
{
    [AmfParam(0)] public int VisitUserId { get; set; }
    [AmfParam(1)] public WorldType Type { get; set; }
}
