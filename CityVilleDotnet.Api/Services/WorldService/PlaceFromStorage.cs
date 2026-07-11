using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public class PlaceFromStorage(CityVilleDbContext context) : AmfService<PlaceFromStorageRequest>
{
    public override async Task<ASObject> HandlePacket(PlaceFromStorageRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x!.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.InventoryItems)
            .ThenInclude(x => x.StoredObject)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");
        if (request.Storage.Length == 0) throw new Exception("Invalid storage request");

        var storageRequest = request.Storage[0];
        var item = player.InventoryItems.FirstOrDefault(x => x.StorageType == storageRequest.Key && x.Name == request.Building.ItemName);

        if (item is null) throw new Exception("Item can't be found from inventory");

        var inventoryItem = player.RemoveItem(request.Building.ItemName, 1, storageRequest.Key);
        var world = player.GetWorld();

        if (item.StoredObject is not null)
        {
            var newBuilding = item.StoredObject.Clone(request.Building.Position.X, request.Building.Position.Y, request.Building.Position.Z, world.GetAvailableBuildingId());

            newBuilding.SetDirection(request.Building.Direction);

            world.Objects.Add(newBuilding);
        }

        if (inventoryItem is not null)
        {
            if (inventoryItem.StoredObject is not null)
            {
                context.Remove(inventoryItem.StoredObject);
            }

            context.Remove(inventoryItem);
        }

        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class PlaceFromStorageRequest
{
    [AmfParam(1)] public BuildingPlaceFromStorageRequest Building { get; set; } = new();
    [AmfParam(3)] public StorageDetails[] Storage { get; set; } = [];
}

public class BuildingPlaceFromStorageRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
    [AmfParam("itemName")] public string ItemName { get; set; } = string.Empty;
    [AmfParam("direction")] public int Direction { get; set; }
}

public class PlaceFromStorageRequestValidator : AbstractValidator<PlaceFromStorageRequest>
{
    public PlaceFromStorageRequestValidator()
    {
        RuleFor(x => x.Building.ItemName).NotEmpty().MaximumLength(64);
    }
}