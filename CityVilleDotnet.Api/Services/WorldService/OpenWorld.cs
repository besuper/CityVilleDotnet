using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class OpenWorld(CityVilleDbContext context, ILogger<OpenWorld> logger) : AmfService<OpenWorldRequest>
{
    public override async Task<ASObject> HandlePacket(OpenWorldRequest request, Guid userId, CancellationToken cancellationToken)
    {
        // Called after visiting friend, might be used to load player world back and move to different player worlds

        // TODO: Update this to support other worlds type (world_main)
        logger.LogDebug("OpenWorld for user {UserId} targeting {OwnerId} world {WorldName}", userId, request.OwnerId, request.WorldName);

        var userToLoad = await context.Set<User>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .Include(x => x.World)
            .ThenInclude(x => x!.MapRects)
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.Player!.Uid == request.OwnerId, cancellationToken);

        if (userToLoad is null)
            throw new Exception($"Unable to find user with Player.Uid {request.OwnerId}");

        var dtoUser = userToLoad.ToDto();

        var response = (ASObject)AmfConverter.Convert(dtoUser.UserInfo);
        response!["franchises"] = new List<object>();
        response["citySim"] = AmfConverter.Convert(dtoUser.UserInfo.World!.CitySim);
        response["featureData"] = AmfConverter.Convert(dtoUser.FeatureData);
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
    [AmfParam(0)] public string OwnerId { get; set; } = string.Empty;
    [AmfParam(1)] public string WorldName { get; set; } = string.Empty;
}