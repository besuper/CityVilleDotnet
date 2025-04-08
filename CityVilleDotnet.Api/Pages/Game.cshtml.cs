using CityVilleDotnet.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityVilleDotnet.Api.Pages;

[Authorize]
public class GameModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUser CurrentUser { get; set; }

    public GameModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        CurrentUser = await _userManager.GetUserAsync(User);

        if (CurrentUser is null)
        {
            return RedirectToPage("/Account/Login");
        }

        return Page();
    }
}
