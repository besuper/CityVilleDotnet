using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityVilleDotnet.Api.Pages.Account;

public class RegisterModel(
    SignInManager<ApplicationUser> signInManager,
    PlayerRegistrationService playerRegistration) : PageModel
{
    [BindProperty] public required RegisterInputModel Input { get; set; }

    public required string ReturnUrl { get; set; } = "/Game";

    public bool DiscordEnabled { get; set; }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : "/Game";
        DiscordEnabled = await signInManager.IsDiscordEnabledAsync();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : "/Game";
        DiscordEnabled = await signInManager.IsDiscordEnabledAsync();

        if (!ModelState.IsValid) return Page();

        var user = new ApplicationUser
        {
            UserName = Input.Username,
        };

        var result = await playerRegistration.RegisterAsync(user, Input.Password);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}
