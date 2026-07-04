using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
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

        // TODO: Update this to support other worlds type (world_main)
        logger.LogDebug("OpenWorld for user {UserId} targeting {OwnerId} world {WorldType}", playerId, request.OwnerId, request.WorldType);

        var playerToLoad = await context.Set<Player>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.World)
            .ThenInclude(x => x!.IncentivizedExpansions)
            .FirstOrDefaultAsync(x => x.Snuid == request.OwnerId, cancellationToken);

        if (playerToLoad is null)
            throw new Exception($"Unable to find player with Player.Uid {request.OwnerId}");

        var dtoUser = playerToLoad.ToDto();

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
            var trackedPlayer = await context.Set<Player>().FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

            if (trackedPlayer is null)
                throw new Exception("Unable to find player");

            trackedPlayer.SwitchWorld(request.WorldType);

            await context.SaveChangesAsync(cancellationToken);
        }

        response!["franchises"] = new List<object>();
        response["citySim"] = AmfConverter.Convert(dtoUser.UserInfo.World!.CitySim);
        response["featureData"] = AmfConverter.Convert(featuredData);
        response["visitDeltas"] = new ASObject();
        response["firstTimeLoaded"] = false;
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