using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Pages;

[Authorize]
public class ItemsModel(CityVilleDbContext dbContext) : PageModel
{
    public List<string?[]> Items { get; } = GameSettingsManager.Instance.GetItems()
        .OrderBy(x => x.Name)
        .Select(x => new[] { x.Name, x.Images.FirstOrDefault(i => i.Name == "icon")?.Url })
        .ToList();

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        var playerId = User.GetPlayerId();

        var player = await dbContext.Set<Player>()
            .AsNoTracking()
            .Where(x => x.Id == playerId)
            .Select(x => new { x.Username, x.Level, x.ProfilePictureUrl })
            .FirstOrDefaultAsync(ct);

        if (player is null)
        {
            return RedirectToPage("/Account/Login");
        }

        ViewData["PlayerName"] = player.Username;
        ViewData["PlayerLevel"] = player.Level;
        ViewData["ProfilePictureUrl"] = player.ProfilePictureUrl ?? "/blank.png";

        return Page();
    }
}
