using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class UpdateEnergy(CityVilleDbContext context, ILogger<UpdateEnergy> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        player.UpdateEnergy();

        await context.SaveChangesAsync(cancellationToken);

        // Global.player.setEnergyFromServer(_loc2_.energy,_loc2_.energyMax,_loc2_.lastEnergyCheck);

        var response = new ASObject
        {
            ["energy"] = player.Energy,
            ["energyMax"] = player.EnergyMax,
            ["lastEnergyCheck"] = player.GetLastCheckEnergyTimestamp()
        };

        logger.LogDebug("UpdateEnergy for user {UserId} with new energy {NewEnergy}", userId, player.Energy);

        return new CityVilleResponse().GameData(response);
    }
}