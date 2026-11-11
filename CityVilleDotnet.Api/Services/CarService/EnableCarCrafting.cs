using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.CarService;

public class EnableCarCrafting(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var carShops = GameSettingsManager.Instance.GetItemsByAddToWorldCallback("Modules.cars.CarManager", "enableCrafting");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => carShops.Contains(o.ItemName)))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var hasCarShop = player.GetWorld().CountBuildingByNames(carShops) > 0;

        if (!hasCarShop || !player.CarsEnabled)
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidState);
        }

        player.EnableCarCrafting();

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}
