using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace CityVilleDotnet.Api.Pages.Account;

public class LoginModel(SignInManager<ApplicationUser> signInManager, IStringLocalizer<Resources.SharedResource> localizer) : PageModel
{
    [BindProperty] public required LoginInputModel Input { get; set; }
    [TempData] public string? ErrorMessage { get; set; }

    public string ReturnUrl { get; set; } = "/Game";

    public bool DiscordEnabled { get; set; }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        DiscordEnabled = await signInManager.IsDiscordEnabledAsync();


        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : "/Game";
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : "/Game";
        DiscordEnabled = await signInManager.IsDiscordEnabledAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await signInManager.PasswordSignInAsync(Input.Username,
            Input.Password,
            Input.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError(string.Empty, localizer["LoginInvalidCredentials"].Value);
        
        return Page();
    }
}