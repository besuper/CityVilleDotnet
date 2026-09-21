using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class Move(CityVilleDbContext context) : AmfService<MoveRequest>
{
    public override async Task<ASObject> HandlePacket(MoveRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.Building.Id || o.TempId == request.Building.Id))
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null) throw new Exception("Player not found");

        var obj = user.GetWorld().GetBuildingByClientId(request.Building.Id) ?? throw new Exception($"Can't find object with id ({request.Building.Id})");

        obj.MoveTo(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z, request.Building.Direction);

        user.HandleQuestsProgress("moveByName", itemName: obj.ItemName);

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
    [AmfParam("direction")] public int Direction { get; set; }
    [AmfParam("id")] public int Id { get; set; }
}