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
        var moveParams = request.MoveParams[0];
        var originX = moveParams.OrigX;
        var originY = moveParams.OrigY;

        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(o => o.X == originX && o.Y == originY))
            .Include(x => x.InventoryItems)
            .Include(x => x.SeenFlags)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null) throw new Exception("Player not found");

        var obj = user.GetWorld().GetBuildingByCoord(originX, originY, 0) ?? throw new Exception($"Can't find object at ({originX}, {originY})");

        obj.MoveTo(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z, request.Building.Direction);

        user.HandleQuestsProgress("moveByName", itemName: obj.ItemName);
        user.CheckCompletedQuests();

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