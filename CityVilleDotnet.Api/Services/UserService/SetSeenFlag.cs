using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SetSeenFlag(CityVilleDbContext context, ILogger<SetSeenFlag> logger) : AmfService<SetSeenFlagRequest>
{
    public override async Task<ASObject> HandlePacket(SetSeenFlagRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Include(x => x.Player)
            .ThenInclude(x => x!.SeenFlags)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Can't to find player with UserId");

        logger.LogDebug("Set seen flag for {FlagName}", request.FlagName);

        player.SetSeenFlag(request.FlagName);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class SetSeenFlagRequest
{
    [AmfParam(0)] public string FlagName { get; set; } = string.Empty;
}