using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class InitialTrees(CityVilleDbContext context) : AmfService<InitialTreesRequest>
{
    public override async Task<ASObject> HandlePacket(InitialTreesRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(w => w.WorldFlatId == request.ObjectId || w.TempId == request.ObjectId))
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var owner = player.GetWorld().GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(owner.ItemName) ?? throw new Exception($"Can't find game item for {owner.ItemName}");

        var mechanic = gameItem.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("initialTrees")
                       ?? throw new Exception($"No initialTrees mechanic found for {owner.ItemName} in game mode {request.GameMode}");

        if (mechanic.ItemNames is null)
            throw new Exception($"No itemNames defined on initialTrees mechanic for {owner.ItemName}");

        foreach (var itemName in mechanic.ItemNames.Split(','))
        {
            player.AddItem(itemName);
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class InitialTreesRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
}
