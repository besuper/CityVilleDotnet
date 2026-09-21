using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Features.Stats.Endpoint;

internal sealed class RecordStats(CityVilleDbContext dbContext) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/record_stats.php");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (HttpContext.User.GetPlayerId() is not { } playerId)
        {
            await Send.OkAsync(cancellation: ct);
            return;
        }
        
        var player = await dbContext.Set<Player>().FirstOrDefaultAsync(x => x.Id == playerId, ct);
        
        if (player is null)
        {
            await Send.OkAsync(cancellation: ct);
            return;
        }
        
        player.UpdateTracking();
        
        await dbContext.SaveChangesAsync(ct);
        
        await Send.OkAsync(cancellation: ct);
    }
}