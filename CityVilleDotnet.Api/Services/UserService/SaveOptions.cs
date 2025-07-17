using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SaveOptions(CityVilleDbContext context) : AmfService
{
    public override async Task<ASObject> HandlePacket(object[] @params, Guid userId, CancellationToken cancellationToken)
    {
        var options = @params[0] as ASObject ?? throw new Exception("Options can't be null");
        var musicDisabled = options["musicDisabled"] as bool? ?? throw new Exception("musicDisabled can't be null");
        var sfxDisabled = options["sfxDisabled"] as bool? ?? throw new Exception("sfxDisabled can't be null");

        var player = await context.Set<User>()
            .Where(x => x.Id == userId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        player.UpdateSettings(musicDisabled, sfxDisabled);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}