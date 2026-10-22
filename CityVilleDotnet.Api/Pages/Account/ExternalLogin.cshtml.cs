using System.Security.Claims;
using System.Text.RegularExpressions;
using AspNet.Security.OAuth.Discord;
using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CityVilleDotnet.Api.Pages.Account;

public class ExternalLoginModel(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    PlayerRegistrationService playerRegistration,
    CityVilleDbContext context,
    ProfilePictureStorage profilePictures,
    IHttpClientFactory httpClientFactory,
    ILogger<ExternalLoginModel> logger,
    IStringLocalizer<Resources.SharedResource> localizer) : PageModel
{
    [BindProperty] public required ExternalLoginInputModel Input { get; set; }

    public string ReturnUrl { get; set; } = "/Game";

    public IActionResult OnGet() => RedirectToPage("./Login");

    public IActionResult OnPost(string? returnUrl = null)
    {
        var redirectUrl = Url.Page("./ExternalLogin", "Callback", new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(DiscordLogin.Scheme, redirectUrl);

        return Challenge(properties, DiscordLogin.Scheme);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : "/Game";

        var info = await signInManager.GetExternalLoginInfoAsync();

        if (remoteError is not null || info is null) return LoginFailed();

        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

        if (result.Succeeded) return LocalRedirect(ReturnUrl);

        Input = new ExternalLoginInputModel
        {
            Username = Regex.Replace(info.Principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty, "[^a-zA-Z0-9]", "")
        };

        return Page();
    }

    public async Task<IActionResult> OnPostConfirmationAsync(string? returnUrl = null)
    {
        ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : "/Game";

        var info = await signInManager.GetExternalLoginInfoAsync();

        if (info is null) return LoginFailed();

        if (!ModelState.IsValid) return Page();

        var user = new ApplicationUser
        {
            UserName = Input.Username,
        };

        await using var transaction = await context.Database.BeginTransactionAsync();

        var result = await playerRegistration.RegisterAsync(user);

        if (result.Succeeded)
        {
            result = await userManager.AddLoginAsync(user, info);
        }

        if (result.Succeeded)
        {
            await transaction.CommitAsync();
            await ApplyDiscordAvatarAsync(user, info);
            await signInManager.SignInAsync(user, isPersistent: true, info.LoginProvider);
            return LocalRedirect(ReturnUrl);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }

    private async Task ApplyDiscordAvatarAsync(ApplicationUser user, ExternalLoginInfo info)
    {
        var avatarHash = info.Principal.FindFirstValue(DiscordAuthenticationConstants.Claims.AvatarHash);

        if (string.IsNullOrEmpty(avatarHash)) return;

        try
        {
            var avatar = await httpClientFactory.CreateClient().GetByteArrayAsync($"https://cdn.discordapp.com/avatars/{info.ProviderKey}/{avatarHash}.png?size=64");

            using var stream = new MemoryStream(avatar);

            var pictureUrl = await profilePictures.SaveAsync(stream, user.UserName!, ".png");

            if (pictureUrl is null) return;

            var player = await context.Set<Player>().FirstAsync(x => x.AppUser!.Id == user.Id);
            player.ProfilePictureUrl = pictureUrl;

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Could not apply the Discord avatar of {UserName}", user.UserName);
        }
    }

    private RedirectToPageResult LoginFailed()
    {
        TempData["ErrorMessage"] = localizer["DiscordLoginFailed"].Value;

        return RedirectToPage("./Login", new { ReturnUrl });
    }
}