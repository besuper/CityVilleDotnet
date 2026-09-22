using CityVilleDotnet.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CityVilleDotnet.Api.Common.Identity;

public static class AdminPolicy
{
    public const string Name = "Admin";
}

public sealed class AdminRequirement : IAuthorizationRequirement;

public sealed class AdminAuthorizationHandler(UserManager<ApplicationUser> userManager) : AuthorizationHandler<AdminRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is { IsAdmin: true })
            context.Succeed(requirement);
    }
}
