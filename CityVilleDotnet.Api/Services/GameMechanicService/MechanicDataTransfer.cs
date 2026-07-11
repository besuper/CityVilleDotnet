using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

internal sealed class MechanicDataTransfer(CityVilleDbContext context) : AmfService<MechanicDataTransferRequest>
{
    public override async Task<ASObject> HandlePacket(MechanicDataTransferRequest request, Guid playerId, CancellationToken cancellationToken)
    {
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

        var slotsMechanic = gameItem.Mechanics?.GetMechanicByGameMode("all")?.GetMechanicItemByType("slots") ?? throw new Exception($"No slots mechanic on {obj.ItemName}");

        var transferredToDisplay = false;

        foreach (var transfer in request.Transfers)
        {
            switch (transfer.Source, transfer.Dest)
            {
                case ("storage", "slots"):
                    obj.RemoveFromStorage(transfer.Target);

                    if (transfer.ExtraParams?.Slot is { } slotIndex)
                        obj.FillSlot(slotIndex, transfer.Target, slotsMechanic.NumSlots);
                    else
                        obj.FillNextSlot(transfer.Target, slotsMechanic.NumSlots);

                    transferredToDisplay = true;
                    break;
                case ("slots", "storage"):
                    var removedItem = transfer.ExtraParams?.Slot is { } slot
                        ? obj.EmptySlot(slot)
                        : throw new Exception("Missing slot index for slots to storage transfer");

                    if (removedItem != transfer.Target)
                        throw new Exception($"Slot content mismatch, expected {transfer.Target} but found {removedItem}");

                    obj.AddToStorage(transfer.Target);
                    break;
                default:
                    throw new Exception($"Unsupported transfer {transfer.Source} to {transfer.Dest}");
            }
        }

        if (transferredToDisplay)
            player.HandleQuestsProgress("transferFromStorageToDisplay", itemName: obj.GetItemName());

        player.HandleQuestsProgress("");
        player.CheckCompletedQuests();

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class MechanicDataTransferRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(1)] public MechanicTransferRequest[] Transfers { get; set; } = [];
}

public class MechanicTransferRequest
{
    [AmfParam("source")] public string Source { get; set; } = string.Empty;
    [AmfParam("dest")] public string Dest { get; set; } = string.Empty;
    [AmfParam("target")] public string Target { get; set; } = string.Empty;
    [AmfParam("extraParams")] public MechanicTransferExtraParams? ExtraParams { get; set; }
}

public class MechanicTransferExtraParams
{
    [AmfParam("slot")] public int? Slot { get; set; }
}
