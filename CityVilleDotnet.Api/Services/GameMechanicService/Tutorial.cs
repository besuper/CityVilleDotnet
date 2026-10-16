using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class Tutorial(CityVilleDbContext context) : AmfService<TutorialRequest>
{
    public override async Task<ASObject> HandlePacket(TutorialRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(w => w.WorldFlatId == request.ObjectId || w.TempId == request.ObjectId))
            .ThenInclude(x => x.MechanicCounters)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");
        
        var obj = player.GetWorld().GetBuildingByClientId(request.ObjectId);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        if (gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("tutorial") is null)
            throw new Exception($"No tutorial mechanic found for {obj.ItemName} in game mode {request.GameMode}");

        obj.IncrementMechanicCounter("tutorial");

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class TutorialRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
}
