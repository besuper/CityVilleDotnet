using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class CancelContract(CityVilleDbContext context) : AmfService<CancelContractRequest>
{
    public override async Task<ASObject> HandlePacket(CancelContractRequest request, Guid playerId, CancellationToken cancellationToken)
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

        if (obj.ContractName is null)
            throw new DomainException(GameErrorType.InvalidState);

        obj.Plow();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class CancelContractRequest
{
    [AmfParam(1)] public BuildingCancelContractRequest Building { get; set; } = new();
}

public class BuildingCancelContractRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}
