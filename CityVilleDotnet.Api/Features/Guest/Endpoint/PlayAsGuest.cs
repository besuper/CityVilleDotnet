using System.Security.Cryptography;
using System.Text;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace CityVilleDotnet.Api.Features.Guest.Endpoint;

internal sealed class PlayAsGuest(UserManager<ApplicationUser> userManager, ILogger<GatewayService> logger, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/Guest");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var enableGuests = configuration.GetValue<bool>("enableGuests");

        if (!enableGuests)
        {
            await Send.RedirectAsync("/Account/Login");
            return;
        }
        
        logger.LogDebug("Creating new guest user");

        var user = new ApplicationUser
        {
            UserName = $"G{new Random().Next(16414029, 97417552)}",
            IsGuest = true
        };

        var result = await userManager.CreateAsync(user, GetMd5(new Random().Next(16414029, 97417552) + DateTimeOffset.Now.ToUnixTimeMilliseconds() + "$"));

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, isPersistent: false);
            await Send.RedirectAsync("/Game");

            return;
        }

        logger.LogDebug("Creating guest account did not succeeded");

        foreach (var identityError in result.Errors)
        {
            logger.LogError("{IdentityErrorCode} {IdentityErrorDescription}", identityError.Code, identityError.Description);
        }

        await Send.RedirectAsync("/Account/Login");
    }

    public static string GetMd5(string input)
    {
        var hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}