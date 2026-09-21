using System.Security.Claims;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CityVilleDotnet.Api.Common.Identity;

public sealed class PlayerClaimsPrincipalFactory(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<IdentityOptions> options,
    CityVilleDbContext context) : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>(userManager, roleManager, options)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var playerId = await context.Set<Player>()
            .AsNoTracking()
            .Where(x => x.AppUser!.Id == user.Id)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (playerId != Guid.Empty)
            identity.AddClaim(new Claim(PlayerClaims.PlayerId, playerId.ToString()));

        return identity;
    }
}
