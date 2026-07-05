using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class SendToStorage(CityVilleDbContext context) : AmfService<SendToStorageRequest>
{
    public override async Task<ASObject> HandlePacket(SendToStorageRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.InventoryItems)
            .ThenInclude(x => x.StoredObject)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();
        var obj = world.GetBuildingByCoord(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z);

        if (obj is null) throw new Exception($"Building is not found at {request.Building.Position.X} {request.Building.Position.Y}");
        if (request.Storage.Length == 0) throw new Exception("Invalid storage request");

        var storageRequest = request.Storage[0];
        var isStackable = obj.GetClassName().IsStackable();

        player.AddItem(obj.GetItemName(), 1, storageRequest.Key, isStackable ? null : obj);

        world.Objects.Remove(obj);

        if (isStackable)
            context.Remove(obj);
        
        player.HandleQuestsProgress("storeItemByClass", className: storageRequest.Key);
        
        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class SendToStorageRequest
{
    [AmfParam(1)] public BuildingSendToStorageRequest Building { get; set; } = new();
    [AmfParam(3)] public StorageDetails[] Storage { get; set; } = [];
}

public class BuildingSendToStorageRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
}