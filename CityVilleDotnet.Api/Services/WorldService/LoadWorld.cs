using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.GameWorlds;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public sealed class LoadWorld(CityVilleDbContext context, ILogger<LoadWorld> logger) : AmfService<LoadWorldRequest>
{
    public override async Task<ASObject> HandlePacket(LoadWorldRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        logger.LogDebug("LoadWorld for user {UserId} targeting {TargetUserId} world {WorldType}", playerId, request.TargetUsedId, request.Type);

        var firstTimeLoaded = await DowntownWorldFactory.EnsureCreatedAsync(context, playerId, request.TargetUsedId, request.Type, cancellationToken);

        var playerToLoad = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == request.Type))
            .ThenInclude(w => w.Objects)
            .ThenInclude(o => o.MechanicCounters)
            .Include(x => x.Worlds.Where(w => w.Type == request.Type))
            .ThenInclude(w => w.MapRects)
            .Include(x => x.Worlds.Where(w => w.Type == request.Type))
            .ThenInclude(w => w.IncentivizedExpansions)
            .FirstOrDefaultAsync(x => x.Snuid == request.TargetUsedId, cancellationToken);

        if (playerToLoad is null)
            throw new Exception($"Unable to find player with Player.Uid {request.TargetUsedId}");

        if (playerToLoad.Id != playerId)
        {
            var currentPlayer = await context.Set<Player>()
                .AsSplitQuery()
                .Include(x => x.Quests)
                .Include(x => x.InventoryItems)
                .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

            if (currentPlayer is null)
                throw new Exception("Current player not found");

            currentPlayer.HandleQuestsProgress("neighborVisit");
            currentPlayer.CheckCompletedQuests();

            await context.SaveChangesAsync(cancellationToken);

            // In-memory only so the DTO targets the requested world, must stay after SaveChangesAsync
            playerToLoad.SwitchWorld(request.Type);
        }
        else
        {
            if (playerToLoad.GetWorldByType(request.Type) is null)
                throw new Exception($"World {request.Type} does not exist for player {playerId}");

            playerToLoad.SwitchWorld(request.Type);

            await context.SaveChangesAsync(cancellationToken);
        }
        
        var allWorlds = await context.Set<World>()
            .AsNoTracking()
            .Where(w => w.Player!.Id == playerToLoad.Id)
            .ToListAsync(cancellationToken);

        var dtoUser = playerToLoad.ToDto(allWorlds);

        var response = (ASObject)AmfConverter.Convert(dtoUser.UserInfo);
        response!["franchises"] = new List<object>();
        response["firstTimeLoaded"] = firstTimeLoaded;

        return new CityVilleResponse().Data(response);
    }
}

public class LoadWorldRequest
{
    [AmfParam(0)] public int TargetUsedId { get; set; }
    [AmfParam(1)] public WorldType Type { get; set; }
}
