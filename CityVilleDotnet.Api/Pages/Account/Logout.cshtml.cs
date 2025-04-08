using CityVilleDotnet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityVilleDotnet.Api.Pages.Account;

public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await _signInManager.SignOutAsync();

        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            return RedirectToPage("/Game");
        }
    }
}
