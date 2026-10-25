using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class Explode(CityVilleDbContext context) : AmfService<ExplodeRequest>
{
    public override async Task<ASObject> HandlePacket(ExplodeRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var owner = world.GetBuildingByClientId(request.ObjectId);

        var gameItem = GameSettingsManager.Instance.GetItem(owner.ItemName) ?? throw new Exception($"Can't find game item for {owner.ItemName}");

        var mechanic = gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("explode")
                       ?? throw new Exception($"No explode mechanic found for {owner.ItemName} in game mode {request.GameMode}");

        if (mechanic.ExplodeToRect is null)
            throw new Exception($"No explodeToRect defined on explode mechanic for {owner.ItemName}");

        var worldRect = GameSettingsManager.Instance.GetWorldRect(mechanic.ExplodeToRect)
                        ?? throw new Exception($"Can't find world rect {mechanic.ExplodeToRect}");

        if (mechanic.GateName is not null)
        {
            foreach (var consumed in player.ConsumeInventoryGate(gameItem, mechanic.GateName))
                context.Set<InventoryItem>().Remove(consumed);
        }

        world.Explode(owner, worldRect, request.Params.TempIds.ToDictionary(x => x.Key, x => Convert.ToInt32(x.Value)));
        context.Remove(owner);

        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class ExplodeRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
    [AmfParam(3)] public ExplodeParamsRequest Params { get; set; } = new();
}

public class ExplodeParamsRequest
{
    [AmfParam("tempIds")] public ASObject TempIds { get; set; } = [];
}
