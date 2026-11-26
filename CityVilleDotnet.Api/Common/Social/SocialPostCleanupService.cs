using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Common.Social;

public class SocialPostCleanupService(IServiceScopeFactory scopeFactory, ILogger<SocialPostCleanupService> logger) : BackgroundService
{
    private static readonly TimeSpan RetentionAfterExpiry = TimeSpan.FromDays(14);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromDays(1));

        do
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var context = scope.ServiceProvider.GetRequiredService<CityVilleDbContext>();
                var limit = ServerUtils.TimeProvider.GetUtcNow().UtcDateTime - RetentionAfterExpiry;

                var deleted = await context.Set<SocialPost>()
                    .Where(x => x.ExpiresAt < limit)
                    .ExecuteDeleteAsync(stoppingToken);

                if (deleted > 0)
                {
                    logger.LogInformation("Deleted {Count} expired social posts", deleted);
                }
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                logger.LogError(e, "Social post cleanup failed");
            }
        } while (await timer.WaitForNextTickAsync(stoppingToken));
    }
}
