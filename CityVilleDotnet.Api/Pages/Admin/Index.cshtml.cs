using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Pages.Admin;

public class IndexModel(CityVilleDbContext dbContext) : PageModel
{
    public int PlayersCount { get; set; }
    public int NewPlayersCount { get; set; }
    public int AdminsCount { get; set; }
    public List<OnlinePlayer> OnlinePlayers { get; set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        var now = DateTimeOffset.Now;
        var onlineThreshold = now - Player.OnlineThreshold;
        var newPlayersThreshold = now.AddDays(-1);

        PlayersCount = await dbContext.Set<Player>().CountAsync(x => x.Snuid != -1, ct);
        NewPlayersCount = await dbContext.Set<Player>().CountAsync(x => x.Snuid != -1 && x.CreationTimestamp >= newPlayersThreshold, ct);
        AdminsCount = await dbContext.Users.CountAsync(x => x.IsAdmin, ct);

        OnlinePlayers = await dbContext.Set<Player>()
            .AsNoTracking()
            .Where(x => x.Snuid != -1 && x.LastTrackingTimestamp >= onlineThreshold)
            .OrderByDescending(x => x.LastTrackingTimestamp)
            .Select(x => new OnlinePlayer(x.Id, x.Snuid, x.Username, x.Level, x.LastPlayedWorldType, x.LastTrackingTimestamp, x.ProfilePictureUrl))
            .ToListAsync(ct);
    }

    public record OnlinePlayer(Guid Id, int Snuid, string Username, int Level, WorldType World, DateTimeOffset LastTrackingTimestamp, string? ProfilePictureUrl);
}
