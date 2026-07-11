using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
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
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .ThenInclude(x => x.Slots)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        var slotsMechanic = gameItem.Mechanics?.GetMechanicByGameMode("all")?.GetMechanicItemByType("slots") ?? throw new Exception($"No slots mechanic on {obj.ItemName}");

        switch (request.Action.Operation)
        {
            case "fillSlot":
            {
                var itemName = Convert.ToString(request.Action.Args[1]) ?? throw new Exception("Missing item name");

                if (!slotsMechanic.AllowsItem(GameSettingsManager.Instance.GetItem(itemName)))
                    throw new Exception($"Item {itemName} is not allowed in slots of {obj.ItemName}");

                obj.FillSlot(Convert.ToInt32(request.Action.Args[0]), itemName, slotsMechanic.NumSlots);
                break;
            }
            case "emptySlot":
                obj.EmptySlot(Convert.ToInt32(request.Action.Args[0]));
                break;
            case "fillNextSlot":
            {
                var itemName = Convert.ToString(request.Action.Args[0]) ?? throw new Exception("Missing item name");

                if (!slotsMechanic.AllowsItem(GameSettingsManager.Instance.GetItem(itemName)))
                    throw new Exception($"Item {itemName} is not allowed in slots of {obj.ItemName}");

                obj.FillNextSlot(itemName, slotsMechanic.NumSlots);
                break;
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
    [AmfParam(3)] public MechanicActionRequest Action { get; set; } = new();
}

public class MechanicActionRequest
{
    [AmfParam("operation")] public string Operation { get; set; } = string.Empty;
    [AmfParam("args")] public object[] Args { get; set; } = [];
}
