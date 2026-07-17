using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Common.GameWorlds;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class OpenWorld(CityVilleDbContext context, ILogger<OpenWorld> logger) : AmfService<OpenWorldRequest>
{
    public override async Task<ASObject> HandlePacket(OpenWorldRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        // Called after visiting friend, might be used to load player world back and move to different player worlds

        logger.LogDebug("OpenWorld for user {UserId} targeting {OwnerId} world {WorldType}", playerId, request.OwnerId, request.WorldType);

        var firstTimeLoaded = await DowntownWorldFactory.EnsureCreatedAsync(context, playerId, request.OwnerId, request.WorldType, cancellationToken);

        var playerToLoad = await context.Set<Player>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == request.WorldType))
            .ThenInclude(w => w.Objects)
            .ThenInclude(o => o.MechanicCounters)
            .Include(x => x.Worlds.Where(w => w.Type == request.WorldType))
            .ThenInclude(w => w.Objects)
            .ThenInclude(o => o.StorageItems)
            .Include(x => x.Worlds.Where(w => w.Type == request.WorldType))
            .ThenInclude(w => w.Objects)
            .ThenInclude(o => o.Slots)
            .Include(x => x.Worlds.Where(w => w.Type == request.WorldType))
            .ThenInclude(w => w.Objects)
            .ThenInclude(o => o.Workers)
            .Include(x => x.Worlds.Where(w => w.Type == request.WorldType))
            .ThenInclude(w => w.MapRects)
            .Include(x => x.Worlds.Where(w => w.Type == request.WorldType))
            .ThenInclude(w => w.IncentivizedExpansions)
            .FirstOrDefaultAsync(x => x.Snuid == request.OwnerId, cancellationToken);

        if (playerToLoad is null)
            throw new Exception($"Unable to find player with Player.Uid {request.OwnerId}");

        playerToLoad.SwitchWorld(request.WorldType);

        logger.LogInformation("OpenWorld sending world {WorldType} with population {Population}/{PopulationCap}",
            playerToLoad.GetWorld().Type, playerToLoad.GetWorld().Population, playerToLoad.GetWorld().PopulationCap);

        // Rows only, the summaries of the non active worlds are built from the persisted fields
        var allWorlds = await context.Set<World>()
            .AsNoTracking()
            .Where(w => w.Player!.Id == playerToLoad.Id)
            .ToListAsync(cancellationToken);

        var dtoUser = playerToLoad.ToDto(allWorlds);

        var response = (ASObject)AmfConverter.Convert(dtoUser.UserInfo);

        // FIXME: Don't remove world in open world for owned worlds otherwise it will clear the map. This cause weird reload in game, might not be the best way
        if (!request.PreloadRequired && playerToLoad.Id != playerId)
        {
            // Remove the world from the response to make Samantha city work, the world is already cached with PreloadWorld
            // Avoid resetting energy from initialVisit
            response!.Remove("world");
        }

        var featuredData = dtoUser.FeatureData;

        if (playerToLoad.IsSamantha())
        {
            // socialInventory feature is enabled after level 10
            // TODO: Check if needed to implement it better
            featuredData["socialInventory"] = new ASObject
            {
                { "samObjectIds", new ASObject(playerToLoad.GetWorld().Objects.ToDictionary(x => x.WorldFlatId.ToString(), _ => (object)0)) }
            };
        }

        if (playerToLoad.Id == playerId)
        {
            var trackedPlayer = await context.Set<Player>()
                .Include(x => x.Quests)
                .Include(x => x.InventoryItems)
                .Include(x => x.Worlds.Where(w => w.Type == request.WorldType))
                .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

            if (trackedPlayer is null)
                throw new Exception("Unable to find player");

            trackedPlayer.SwitchWorld(request.WorldType);
            trackedPlayer.SpawnEligibleQuests();
            
            trackedPlayer.HandleQuestsProgress("travel", itemName: request.WorldType.ToDescriptionString());

            await context.SaveChangesAsync(cancellationToken);
        }

        response!["franchises"] = new List<object>();
        response["citySim"] = AmfConverter.Convert(dtoUser.UserInfo.World!.CitySim);
        response["featureData"] = AmfConverter.Convert(featuredData);
        response["visitDeltas"] = new ASObject();
        response["firstTimeLoaded"] = firstTimeLoaded;
        response["crews"] = null;
        response["ugc"] = null;
        response["orders"] = new List<object>();

        return new CityVilleResponse().Data(response);
    }
}

public class OpenWorldRequest
{
    [AmfParam(0)] public int OwnerId { get; set; }
    [AmfParam(1)] public WorldType WorldType { get; set; }
    [AmfParam(3)] public bool PreloadRequired { get; set; }
}
