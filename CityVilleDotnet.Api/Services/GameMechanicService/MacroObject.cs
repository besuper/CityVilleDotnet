using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class MacroObject(CityVilleDbContext context) : AmfService<MacroObjectRequest>
{
    public override async Task<ASObject> HandlePacket(MacroObjectRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (request.Params.Operation != "performExplode")
            throw new Exception($"Unsupported macroObject operation {request.Params.Operation}");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var owner = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(owner.ItemName) ?? throw new Exception($"Can't find game item for {owner.ItemName}");

        var mechanic = gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("macroObject")
                       ?? throw new Exception($"No macroObject mechanic found for {owner.ItemName} in game mode {request.GameMode}");

        if (mechanic.ExplodeToRect is null || mechanic.MacroPrefix is null)
            throw new Exception($"No explodeToRect or macroPrefix defined on macroObject mechanic for {owner.ItemName}");

        var worldRect = GameSettingsManager.Instance.GetWorldRect(mechanic.ExplodeToRect)
                        ?? throw new Exception($"Can't find world rect {mechanic.ExplodeToRect}");

        if (mechanic.GateName is not null)
        {
            foreach (var consumed in player.ConsumeInventoryGate(gameItem, mechanic.GateName))
                context.Set<InventoryItem>().Remove(consumed);
        }

        var children = world.Explode(owner, worldRect, request.Params.TempIds.ToDictionary(x => x.Key, x => Convert.ToInt32(x.Value)));
        world.CreateMacroObject(mechanic.MacroPrefix, owner.ItemName, children);
        context.Remove(owner);

        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class MacroObjectRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
    [AmfParam(3)] public MacroObjectParamsRequest Params { get; set; } = new();
}

public class MacroObjectParamsRequest
{
    [AmfParam("operation")] public string Operation { get; set; } = string.Empty;
    [AmfParam("tempIds")] public ASObject TempIds { get; set; } = [];
}
