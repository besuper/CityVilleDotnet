using CityVilleDotnet.Persistence;

namespace CityVilleDotnet.Api.Features.Guest.Services;

public class CleanGuestsService(ILogger<CleanGuestsService> logger, CityVilleDbContext context) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Clean guests service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessTransactionsAsync();

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }

        logger.LogInformation("Clean guests service stopping.");
    }

    private Task ProcessTransactionsAsync()
    {
        Console.WriteLine("Processing transactions...");
        return Task.CompletedTask;
    }
}