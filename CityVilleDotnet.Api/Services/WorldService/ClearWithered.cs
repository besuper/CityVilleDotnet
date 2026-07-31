using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class ClearWithered(CityVilleDbContext context) : AmfService<ClearWitheredRequest>
{
    public override async Task<ASObject> HandlePacket(ClearWitheredRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(o => o.X == request.Building.Position.X && o.Y == request.Building.Position.Y))
            .ThenInclude(x => x.Workers)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z);

        if (obj is null)
            throw new Exception("Can't find building with coords");

        if (!obj.IsWithered())
            throw new DomainException(GameErrorType.InvalidState);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.GetItemName());

        if (gameItem is null)
            throw new Exception($"Can't find game item for {obj.GetItemName()}");

        var refund = (int)((gameItem.Cost ?? 0) * GameSettingsManager.Instance.GetSettings().WitherRefundMultiplier);

        if (refund > 0)
            player.AddCoins(refund);

        obj.Plow();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class ClearWitheredRequest
{
    [AmfParam(1)] public BuildingClearWitheredRequest Building { get; set; } = new();
}

public class BuildingClearWitheredRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}
