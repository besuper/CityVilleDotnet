using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class LinkedObjectMoveCompact(CityVilleDbContext context) : AmfService<LinkedObjectMoveCompactRequest>
{
    public override async Task<ASObject> HandlePacket(LinkedObjectMoveCompactRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var linkedObjects = request.Params[0].LinkedObjects;
        var ids = linkedObjects.Select(x => x.Id).ToList();

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => ids.Contains(o.WorldFlatId) || ids.Contains(o.TempId)))
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        // The first linked object is the parent, the one picked by the player
        WorldObject? parent = null;

        foreach (var linkedObject in linkedObjects)
        {
            var obj = world.GetBuildingByClientId(linkedObject.Id) ?? throw new DomainException(GameErrorType.ForceReload);

            obj.MoveTo(linkedObject.X, linkedObject.Y, obj.Z ?? 0, linkedObject.Direction);

            parent ??= obj;
        }

        player.HandleQuestsProgress("moveByName", itemName: parent!.ItemName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class LinkedObjectMoveCompactRequest
{
    [AmfParam(3)] public LinkedObjectMoveCompactParamsRequest[] Params { get; set; } = [];
}

public class LinkedObjectMoveCompactParamsRequest
{
    [AmfParam("linkedObjects")] public LinkedObjectRequest[] LinkedObjects { get; set; } = [];
}

public class LinkedObjectRequest
{
    [AmfParam("x")] public int X { get; set; }
    [AmfParam("y")] public int Y { get; set; }
    [AmfParam("direction")] public int Direction { get; set; }
    [AmfParam("id")] public int Id { get; set; }
}

public class LinkedObjectMoveCompactValidator : AbstractValidator<LinkedObjectMoveCompactRequest>
{
    public LinkedObjectMoveCompactValidator()
    {
        RuleFor(x => x.Params).NotEmpty();
        RuleForEach(x => x.Params).ChildRules(p => p.RuleFor(x => x.LinkedObjects).NotEmpty());
    }
}
