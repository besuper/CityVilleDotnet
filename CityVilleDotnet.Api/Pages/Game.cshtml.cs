using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using CityVilleDotnet.Common.Utils;

namespace CityVilleDotnet.Api.Pages;

[Authorize]
public class GameModel(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext) : PageModel
{
    public string FriendList { get; set; } = "[]";
    public string Uid { get; set; } = "333";
    public string UserName { get; set; } = "Steve";
    public int Level { get; set; } = 1;
    public long ServerTime { get; set; } = 0;

    public async Task<IActionResult> OnGetAsync()
    {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser is null)
            return RedirectToPage("/Account/Login");

        var user = await dbContext.Set<User>()
            .AsNoTracking()
            .Include(x => x.AppUser)
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.AppUser!.Id.Equals(currentUser.Id));
        
        ServerTime = ServerUtils.GetCurrentTime();

        if (user?.Player is not null)
        {
            Uid = user.Player.Uid;
            UserName = user.Player.Username;
            Level = user.Player.Level;
            FriendList = JsonSerializer.Serialize(user.GetFriendsData(), new JsonSerializerOptions { WriteIndented = false, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        }
        else
        {
            UserName = currentUser.UserName ?? "Unknown";
        }

        return Page();
    }
}