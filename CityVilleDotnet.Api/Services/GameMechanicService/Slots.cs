using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

internal sealed class Slots(CityVilleDbContext context) : AmfService<SlotsRequest>
{
    public override async Task<ASObject> HandlePacket(SlotsRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId || o.WorldFlatId == request.Action.ObjectId || o.TempId == request.Action.ObjectId))
            .ThenInclude(x => x.Slots)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.ObjectId);

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        var slotsMechanic = gameItem.Mechanics?.GetMechanicByGameMode("all")?.GetMechanicItemByType("slots");

        switch (request.Action.Operation)
        {
            case "fillSlot":
            {
                var args = request.Action.Args ?? throw new Exception("Missing args");
                var itemName = Convert.ToString(args[1]) ?? throw new Exception("Missing item name");

                if (slotsMechanic?.AllowsItem(GameSettingsManager.Instance.GetItem(itemName)) != true)
                {
                    throw new Exception($"Item {itemName} is not allowed in slots of {obj.ItemName}");
                }

                obj.FillSlot(Convert.ToInt32(args[0]), itemName, slotsMechanic.NumSlots);
                break;
            }
            case "emptySlot":
                obj.EmptySlot(Convert.ToInt32((request.Action.Args ?? throw new Exception("Missing args"))[0]));
                break;
            case "fillNextSlot":
            {
                var args = request.Action.Args ?? throw new Exception("Missing args");
                var itemName = Convert.ToString(args[0]) ?? throw new Exception("Missing item name");

                if (slotsMechanic?.AllowsItem(GameSettingsManager.Instance.GetItem(itemName)) != true)
                {
                    throw new Exception($"Item {itemName} is not allowed in slots of {obj.ItemName}");
                }

                obj.FillNextSlot(itemName, slotsMechanic.NumSlots);
                break;
            }
            case "buildInPlace":
            {
                var storage = gameItem.ThemeParkStorage ?? throw new Exception($"No theme park storage on {obj.ItemName}");
                var slotIndex = request.Action.SlotIndex ?? throw new Exception("Missing slot index");
                var slotItemName = storage.GetSlotItemName(slotIndex, "inplace");
                var slotItem = GameSettingsManager.Instance.GetItem(slotItemName) ?? throw new Exception($"Can't find game item for {slotItemName}");

                obj.FillSlot(slotIndex, slotItemName, storage.Slots.Count);

                foreach (var consumed in player.ConsumeInventoryGate(slotItem, "build"))
                {
                    context.Set<InventoryItem>().Remove(consumed);
                }

                break;
            }
            case "storeObject":
            {
                var storage = gameItem.ThemeParkStorage ?? throw new Exception($"No theme park storage on {obj.ItemName}");
                var slotIndex = request.Action.SlotIndex ?? throw new Exception("Missing slot index");
                var stored = world.GetBuildingByClientId(request.Action.ObjectId ?? throw new Exception("Missing object id")) ?? throw new Exception($"Can't find building with id {request.Action.ObjectId}");

                if (stored.ItemName != storage.GetSlotItemName(slotIndex, "storage"))
                {
                    throw new Exception($"Item {stored.ItemName} can't be stored in slot {slotIndex} of {obj.ItemName}");
                }

                obj.FillSlot(slotIndex, stored.ItemName, storage.Slots.Count);

                world.RemoveBuilding(stored);
                context.Set<WorldObject>().Remove(stored);
                break;
            }
            case "placeObjectFromStorage":
            {
                var storage = gameItem.ThemeParkStorage ?? throw new Exception($"No theme park storage on {obj.ItemName}");
                var slotIndex = request.Action.SlotIndex ?? throw new Exception("Missing slot index");
                var data = request.Action.ObjectData ?? throw new Exception("Missing object data");

                if (data.ItemName != storage.GetSlotItemName(slotIndex, "storage"))
                {
                    throw new Exception($"Item {data.ItemName} can't be placed from slot {slotIndex} of {obj.ItemName}");
                }

                obj.EmptySlot(slotIndex);

                var placed = new WorldObject(data.ItemName, data.ClassName, null, false, data.Id, data.State, data.Direction,
                    ServerUtils.GetCurrentTime(), ServerUtils.GetCurrentTime(), data.Position.X, data.Position.Y, data.Position.Z, world.GetAvailableBuildingId());

                placed.InitHarvestEndTime();
                world.AddBuilding(placed);

                await context.SaveChangesAsync(cancellationToken);

                return new CityVilleResponse().Data(new ASObject { ["objectId"] = placed.WorldFlatId });
            }
            default:
                throw new Exception($"Unsupported slots operation {request.Action.Operation}");
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class SlotsRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(3)] public SlotsActionRequest Action { get; set; } = new();
}

public class SlotsActionRequest
{
    [AmfParam("operation")] public string Operation { get; set; } = string.Empty;
    [AmfParam("args")] public object[]? Args { get; set; }
    [AmfParam("slotIndex")] public int? SlotIndex { get; set; }
    [AmfParam("objectId")] public int? ObjectId { get; set; }
    [AmfParam("objectData")] public BuildingPlaceRequest? ObjectData { get; set; }
}

public class MechanicActionRequest
{
    [AmfParam("operation")] public string Operation { get; set; } = string.Empty;
    [AmfParam("args")] public object[] Args { get; set; } = [];
}
