using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.ViewComponents;

public class SocialNotificationsViewComponent(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(bool friendRequests)
    {
        var appUserId = userManager.GetUserId(UserClaimsPrincipal);

        var player = await dbContext.Set<Player>()
            .Where(x => x.AppUser!.Id == appUserId)
            .Select(x => new { x.Id, x.SocialNotificationsSeenAt })
            .FirstOrDefaultAsync();

        if (player is null)
        {
            return View(0);
        }

        if (friendRequests)
        {
            return View(await dbContext.Set<Friend>().CountAsync(x => x.Player!.Id == player.Id && x.Status == FriendshipStatus.Pending && x.Requested));
        }

        var now = ServerUtils.TimeProvider.GetUtcNow().UtcDateTime;

        var newHelps = await dbContext.Set<SocialPost>()
            .Where(x => x.Player!.Id == player.Id)
            .SelectMany(x => x.Helpers)
            .CountAsync(x => x.HelpedAt > player.SocialNotificationsSeenAt);

        var pendingReceived = await dbContext.Set<SocialPost>()
            .CountAsync(x => x.Type != SocialPostType.Feed && x.TargetPlayer!.Id == player.Id && x.ExpiresAt > now && !x.Helpers.Any());

        return View(newHelps + pendingReceived);
    }
}