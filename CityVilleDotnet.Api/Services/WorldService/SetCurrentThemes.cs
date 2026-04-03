using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.WorldService;

public sealed class SetCurrentThemes(CityVilleDbContext context) : AmfService<SetCurrentThemesRequest>
{
    public override async Task<ASObject> HandlePacket(SetCurrentThemesRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.World)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var world = player.GetWorld();

        world.UpdateTheme(request.ThemeName, request.Enabled);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class SetCurrentThemesRequest
{
    [AmfParam(0)] public string ThemeName { get; set; } = string.Empty;
    [AmfParam(1)] public bool Enabled { get; set; }
}