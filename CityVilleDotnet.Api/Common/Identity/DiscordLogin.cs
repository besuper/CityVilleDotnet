using AspNet.Security.OAuth.Discord;
using CityVilleDotnet.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CityVilleDotnet.Api.Common.Identity;

public static class DiscordLogin
{
    public const string Scheme = DiscordAuthenticationDefaults.AuthenticationScheme;

    public static async Task<bool> IsDiscordEnabledAsync(this SignInManager<ApplicationUser> signInManager)
    {
        var schemes = await signInManager.GetExternalAuthenticationSchemesAsync();

        return schemes.Any(x => x.Name == Scheme);
    }
}
