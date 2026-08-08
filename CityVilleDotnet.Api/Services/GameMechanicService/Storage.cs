using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

internal sealed class Storage(CityVilleDbContext context) : AmfService<StorageRequest>
{
    public override async Task<ASObject> HandlePacket(StorageRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (request.Action.Operation != "purchase")
            throw new Exception($"Unsupported storage operation {request.Action.Operation}");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .ThenInclude(x => x.StorageItems)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .ThenInclude(x => x.Slots)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        var obj = world.GetBuildingByClientId(request.ObjectId) ?? throw new Exception($"Can't find building with id {request.ObjectId}");

        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        var storageMechanic = gameItem.Mechanics?.GetMechanicByGameMode("all")?.GetMechanicItemByType("storage") ?? throw new Exception($"No storage mechanic on {obj.ItemName}");

        var itemName = Convert.ToString(request.Action.Args[0]) ?? throw new Exception("Missing item name");
        var amount = Convert.ToInt32(request.Action.Args[1]);

        if (amount <= 0)
            throw new Exception($"Invalid purchase amount {amount}");

        var purchasedItem = GameSettingsManager.Instance.GetItem(itemName) ?? throw new Exception($"Purchased item not found {itemName}");

        if (!storageMechanic.AllowsItem(purchasedItem))
            throw new Exception($"Item {itemName} is not allowed in storage of {obj.ItemName}");

        player.RemoveCash((purchasedItem.Cash ?? 0) * amount);

        obj.AddToStorage(itemName, amount);

        player.HandleQuestsProgress("");

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class StorageRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(3)] public MechanicActionRequest Action { get; set; } = new();
}
