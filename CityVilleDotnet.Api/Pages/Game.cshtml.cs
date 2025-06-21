using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace CityVilleDotnet.Api.Pages;

[Authorize]
public class GameModel(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext) : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly CityVilleDbContext _dbContext = dbContext;

    public required ApplicationUser CurrentUser { get; set; }
    public required User CurrentPlayer { get; set; }
    public string FriendList { get; set; } = "[]";

    public async Task<IActionResult> OnGetAsync()
    {
        CurrentUser = await _userManager.GetUserAsync(User);

        if (CurrentUser is null)
            return RedirectToPage("/Account/Login");

        var user = await _dbContext.Set<User>()
            .AsNoTracking()
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.AppUser.Id.Equals(CurrentUser.Id));

        if(user is not null)
        {
            CurrentPlayer = user;
            
            var options = new JsonSerializerOptions() { WriteIndented = false, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

            FriendList = JsonSerializer.Serialize(user.GetFriendsData(), options);
        }

        return Page();
    }
}
