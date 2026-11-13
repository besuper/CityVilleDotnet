using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
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

        // client always sends deco_2, items may still be stored under a 2012 building key
        var item = player.InventoryItems.FirstOrDefault(x => x.StorageType is not null && x.Name == request.Building.ItemName);

        if (item is null) throw new Exception("Item can't be found from inventory");

        var inventoryItem = player.RemoveItem(request.Building.ItemName, 1, item.StorageType);
        var world = player.GetWorld();

        var building = request.Building;
        var objId = world.GetAvailableBuildingId();

        if (item.StoredObject is null && !building.ClassName.IsStackable())
        {
            throw new Exception($"Stored item {building.ItemName} has no stored object");
        }

        var newBuilding = item.StoredObject?.Clone(building.Id, building.Position.X, building.Position.Y, building.Position.Z, objId)
                          ?? new WorldObject(building.ItemName, building.ClassName, null, false, building.Id, building.State, building.Direction, null, ServerUtils.GetCurrentTime(), building.Position.X, building.Position.Y, building.Position.Z, objId);

        newBuilding.SetDirection(building.Direction);

        world.AddBuilding(newBuilding);

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
}

public class BuildingPlaceFromStorageRequest
{
    [AmfParam("position")] public PerformActionPositionRequest Position { get; set; } = new();
    [AmfParam("itemName")] public string ItemName { get; set; } = string.Empty;
    [AmfParam("direction")] public int Direction { get; set; }
    [AmfParam("id")] public int Id { get; set; }
    [AmfParam("className")] public BuildingClassType ClassName { get; set; }
    [AmfParam("state")] public WorldObjectState State { get; set; }
}

public class PlaceFromStorageRequestValidator : AbstractValidator<PlaceFromStorageRequest>
{
    public PlaceFromStorageRequestValidator()
    {
        RuleFor(x => x.Building.ItemName).NotEmpty().MaximumLength(64);
    }
}