using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class UpdateEnergy(CityVilleDbContext context, ILogger<UpdateEnergy> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.World)
            .ThenInclude(x => x!.Objects.Where(o => o.StreakLength > 0))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        player.UpdateEnergy();

        await context.SaveChangesAsync(cancellationToken);

        // Global.player.setEnergyFromServer(_loc2_.energy,_loc2_.energyMax,_loc2_.lastEnergyCheck);

        logger.LogDebug("UpdateEnergy for user {UserId} with new energy {NewEnergy}", playerId, player.Energy);

        return new CityVilleResponse().GameData(new ASObject
        {
            ["energy"] = player.Energy,
            ["energyMax"] = player.EnergyMax,
            ["lastEnergyCheck"] = player.GetLastCheckEnergyTimestamp()
        });
    }
}