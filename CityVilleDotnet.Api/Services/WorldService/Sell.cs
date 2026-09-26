using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

internal sealed class Sell(CityVilleDbContext context) : AmfService<SellRequest>
{
    public override async Task<ASObject> HandlePacket(SellRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var user = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.FranchiseLocation)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.MechanicCounters)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.StorageItems)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .ThenInclude(x => x.Slots)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (user is null) throw new Exception("Player not found");

        var world = user.GetWorld();

        var obj = world.GetBuildingByClientId(request.Building.Id) ?? throw new Exception($"Can't find building with id {request.Building.Id}");
        
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName);

        if (gameItem is null) throw new Exception($"Can't find item with name {obj.ItemName}");

        world.RemoveBuilding(obj);

        if (obj.FranchiseLocation is not null && obj.ItemOwner is not null)
        {
            var sender = await context.Set<Player>()
                .AsSplitQuery()
                .Include(x => x.Franchises)
                .ThenInclude(x => x.Locations)
                .FirstOrDefaultAsync(x => x.Snuid.ToString() == obj.ItemOwner, cancellationToken);

            if (sender is not null)
            {
                sender.RemoveFranchiseLocation(user.Snuid.ToString(), obj.WorldFlatId);
            }
        }

        context.Set<WorldObject>().Remove(obj);

        if (gameItem.IsSellSendsToInventory)
            user.AddItem(obj.ItemName);
        else
            user.AddCoins(obj.GetSellPrice());

        world.CalculatePopulation();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class SellRequest
{
    [AmfParam(1)] public SellBuildingRequest Building { get; set; } = new();
}

public class SellBuildingRequest
{
    [AmfParam("id")] public int Id { get; set; }
}