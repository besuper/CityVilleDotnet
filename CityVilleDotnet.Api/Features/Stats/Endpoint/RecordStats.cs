using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Features.Stats.Endpoint;

internal sealed class RecordStats(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/record_stats.php");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var currentUser = await userManager.GetUserAsync(HttpContext.User);

        if (currentUser is null)
        {
            await Send.OkAsync(cancellation: ct);
            return;
        }
        
        var player = await dbContext.Set<Player>().FirstOrDefaultAsync(x => x.AppUser!.Id == currentUser.Id, ct);
        
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