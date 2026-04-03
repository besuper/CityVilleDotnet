using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class Move(CityVilleDbContext context) : AmfService<MoveRequest>
{
    public override async Task<ASObject> HandlePacket(MoveRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var moveParams = request.MoveParams[0];
        var originX = moveParams.OrigX;
        var originY = moveParams.OrigY;

        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.Player)
            .ThenInclude(x => x!.World)
            .ThenInclude(x => x!.Objects.Where(o => o.X == originX && o.Y == originY))
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null) throw new Exception($"User not found with id {userId}");

        var obj = user.GetPlayer().GetWorld().GetBuildingByCoord(originX, originY, 0) ?? throw new Exception($"Can't find object at ({originX}, {originY})");

        obj.MoveTo(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z, request.Building.Direction);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class MoveRequest
{
    [AmfParam(1)] public MoveBuildingRequest Building { get; set; } = new();
    [AmfParam(3)] public MoveParamsRequest[] MoveParams { get; set; } = [];
}

public class MoveBuildingRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
    [AmfParam("direction")] public int Direction { get; set; }
}

public class MoveParamsRequest
{
    [AmfParam("origX")] public int OrigX { get; set; }
    [AmfParam("origY")] public int OrigY { get; set; }
}