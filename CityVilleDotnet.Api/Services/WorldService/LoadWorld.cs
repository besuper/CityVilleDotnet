using CityVilleDotnet.Api.Common.Amf;
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
        var playerToLoad = await context.Set<Player>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
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
        }
        else
        {
            var trackedPlayer = await context.Set<Player>().FirstOrDefaultAsync(x => x.Snuid == request.TargetUsedId, cancellationToken);

            if (trackedPlayer is null)
                throw new Exception($"Unable to find player with Player.Uid {request.TargetUsedId}");

            trackedPlayer.SwitchWorld(request.Type);
            playerToLoad.SwitchWorld(request.Type);
        }

        var dtoUser = playerToLoad.ToDto();

        var response = (ASObject)AmfConverter.Convert(dtoUser.UserInfo);
        response!["franchises"] = new List<object>();

        return new CityVilleResponse().Data(response);
    }
}

public class LoadWorldRequest
{
    [AmfParam(0)] public int TargetUsedId { get; set; }
    [AmfParam(1)] public WorldType Type { get; set; }
}