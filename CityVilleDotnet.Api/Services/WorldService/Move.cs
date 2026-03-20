using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Move(CityVilleDbContext context) : AmfService<MoveRequest>
{
    public override async Task<ASObject> HandlePacket(MoveRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (user is null) throw new Exception($"User not found with id {userId}");

        var obj = user.GetWorld().GetBuildingById(request.Building.Id) ?? throw new Exception($"Can't find object with id {request.Building.Id}");

        obj.MoveTo(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z, request.Building.Direction);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class MoveRequest
{
    [AmfParam(1)] public MoveBuildingRequest Building { get; set; } = new();
}

public class MoveBuildingRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
    [AmfParam("id")] public int Id { get; set; }
    [AmfParam("direction")] public int Direction { get; set; }
}