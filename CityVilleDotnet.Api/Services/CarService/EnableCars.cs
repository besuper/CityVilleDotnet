using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.CarService;

public class EnableCars(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var carBuildings = GameSettingsManager.Instance.GetItemsByAddToWorldCallback("Modules.cars.CarManager", "enableCars", "enableCrafting");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Cars)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => carBuildings.Contains(o.ItemName)))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        if (player.GetWorld().CountBuildingByNames(carBuildings) == 0 || !player.EnableCars())
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidState);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}
