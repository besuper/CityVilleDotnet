using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class Toaster(CityVilleDbContext context) : AmfService<ToasterRequest>
{
    public override async Task<ASObject> HandlePacket(ToasterRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects.Where(w => w.WorldFlatId == request.ObjectId || w.TempId == request.ObjectId))
            .ThenInclude(x => x.MechanicCounters)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var owner = player.GetWorld().GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(owner.ItemName) ?? throw new Exception($"Can't find game item for {owner.ItemName}");

        if (gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("toaster") is null)
            throw new Exception($"No toaster mechanic found for {owner.ItemName} in game mode {request.GameMode}");

        owner.IncrementMechanicCounter("toaster");

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class ToasterRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
}
