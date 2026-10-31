using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WeatherService;

public class SetWeatherEffect(CityVilleDbContext context) : AmfService<SetWeatherEffectRequest>
{
    public override async Task<ASObject> HandlePacket(SetWeatherEffectRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.SetWeatherEffectDisabled(request.DisableWeatherEffect);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class SetWeatherEffectRequest
{
    [AmfParam(0)] public bool DisableWeatherEffect { get; set; }
}
