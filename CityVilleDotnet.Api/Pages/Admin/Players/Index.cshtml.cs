using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Pages.Admin.Players;

public class IndexModel(CityVilleDbContext dbContext) : PageModel
{
    private const int PageSize = 25;
    private const int MaxSearchLength = 32;

    [BindProperty(SupportsGet = true)] public string? Search { get; set; }
    [BindProperty(SupportsGet = true)] public int CurrentPage { get; set; } = 1;

    public List<PlayerRow> Players { get; set; } = [];
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public DateTimeOffset OnlineThreshold { get; } = DateTimeOffset.Now - Player.OnlineThreshold;

    public async Task OnGetAsync(CancellationToken ct)
    {
        CurrentPage = Math.Max(1, CurrentPage);

        var query = dbContext.Set<Player>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var search = Search.Trim();

            if (search.Length > MaxSearchLength)
                search = search[..MaxSearchLength];

            query = int.TryParse(search, out var snuid)
                ? query.Where(x => x.Snuid == snuid || x.Username.Contains(search))
                : query.Where(x => x.Username.Contains(search));
        }

        TotalCount = await query.CountAsync(ct);

        Players = await query
            .OrderByDescending(x => x.LastTrackingTimestamp)
            .ThenBy(x => x.Username)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .Select(x => new PlayerRow(x.Id, x.Snuid, x.Username, x.Level, x.Gold, x.Cash, x.CreationTimestamp, x.LastTrackingTimestamp, x.AppUser != null && x.AppUser.IsAdmin, x.ProfilePictureUrl))
            .ToListAsync(ct);
    }

    public record PlayerRow(Guid Id, int Snuid, string Username, int Level, int Gold, int Cash, DateTimeOffset CreationTimestamp, DateTimeOffset LastTrackingTimestamp, bool IsAdmin, string? ProfilePictureUrl);
}
