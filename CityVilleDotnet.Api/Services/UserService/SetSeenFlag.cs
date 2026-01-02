using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SetSeenFlag(CityVilleDbContext context, ILogger<SetSeenFlag> logger) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Can't to find player with UserId");

        var flagName = (string)@params[0] ?? throw new Exception("Flag name can't be null");

        logger.LogDebug("Set seen flag for {FlagName}", flagName);

        player.SetSeenFlag(flagName);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}