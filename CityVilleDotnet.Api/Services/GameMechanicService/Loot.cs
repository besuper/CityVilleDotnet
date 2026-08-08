using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public sealed class Loot(CityVilleDbContext context) : AmfService<LootRequest>
{
    public override async Task<ASObject> HandlePacket(LootRequest request, Guid playerId, CancellationToken cancellationToken)
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

        if (obj.GetClassName() != BuildingClassType.ZooEnclosure)
            throw new Exception($"Can't loot a random animal on {obj.ItemName}");

        var price = GameSettingsManager.Instance.GetSettings().GetZooDonationNpcPrice(obj.GetItemName());

        player.RemoveCash(price);

        var animalName = obj.RollRandomZooAnimal();

        player.HandleQuestsProgress("");

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["loot"] = animalName
        });
    }
}

public class LootRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
}
