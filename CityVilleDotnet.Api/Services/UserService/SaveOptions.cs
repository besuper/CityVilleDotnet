using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SaveOptions(CityVilleDbContext context) : AmfService<SaveOptionsRequest>
{
    public override async Task<ASObject> HandlePacket(SaveOptionsRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var player = await context.Set<User>()
            .Where(x => x.UserId == userId)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(cancellationToken);

        if (player is null) throw new Exception("Can't find player with UserId");

        player.UpdateSettings(request.Options.MusicDisabled, request.Options.SfxDisabled);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class SaveOptionsParams
{
    [AmfParam("musicDisabled")] public bool MusicDisabled { get; set; }
    [AmfParam("sfxDisabled")] public bool SfxDisabled { get; set; }
}

public class SaveOptionsRequest
{
    [AmfParam(0)] public SaveOptionsParams Options { get; set; } = new();
}