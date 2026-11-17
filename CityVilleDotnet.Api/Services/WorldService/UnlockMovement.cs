using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class UnlockMovement(CityVilleDbContext context) : AmfService<UnlockMovementRequest>
{
    public override async Task<ASObject> HandlePacket(UnlockMovementRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.Building.Id || o.TempId == request.Building.Id || o.EnergyModifier > 0))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.Building.Id);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem?.EnergyCost?.UnlockMovement is not null)
        {
            player.RemoveEnergy(int.Parse(gameItem.EnergyCost.UnlockMovement));
        }

        obj.UnlockMovement();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class UnlockMovementRequest
{
    [AmfParam(1)] public UnlockMovementBuildingRequest Building { get; set; } = new();
}

public class UnlockMovementBuildingRequest
{
    [AmfParam("id")] public int Id { get; set; }
}
