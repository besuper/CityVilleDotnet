using System.Globalization;
using CityVilleDotnet.Api.Common.Social;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CityVilleDotnet.Api.Pages.Social;

[Authorize]
public class WallModel(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext, FeedTextRenderer feedTextRenderer, IStringLocalizer<Resources.SharedResource> localizer) : PageModel
{
    public const string FriendsTab = "friends";
    public const string CommunityTab = "community";
    public const string MineTab = "mine";

    public string Tab { get; private set; } = FriendsTab;
    public int CurrentPage { get; private set; } = 1;
    public int TotalPages { get; private set; }
    public List<WallPost> Posts { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(string? tab, int pageNumber = 1, CancellationToken ct = default)
    {
        var player = await GetCurrentPlayerAsync(ct);

        if (player is null)
        {
            return RedirectToPage("/Account/Login");
        }

        var now = ServerUtils.TimeProvider.GetUtcNow().UtcDateTime;
        var friendIds = await GetAcceptedFriendIdsAsync(player.Id, ct);

        if (tab is CommunityTab or MineTab or FriendsTab)
        {
            Tab = tab;
        }
        else if (await HasUnseenHelpsAsync(player, ct))
        {
            Tab = MineTab;
        }
        else
        {
            Tab = FriendsTab;
        }

        CurrentPage = Math.Max(1, pageNumber);

        var query = dbContext.Set<SocialPost>().AsNoTracking();

        query = Tab switch
        {
            MineTab => query.Where(x => x.Player!.Id == player.Id),
            CommunityTab => query.Where(x => x.ExpiresAt > now && x.Player!.Id != player.Id && !friendIds.Contains(x.Player.Id)),
            _ => query.Where(x => x.ExpiresAt > now && friendIds.Contains(x.Player!.Id))
        };

        var pageSize = 12;

        var totalCount = await query.CountAsync(ct);

        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var posts = await query
            .AsSplitQuery()
            .Include(x => x.Player)
            .Include(x => x.TargetPlayer)
            .Include(x => x.Helpers.OrderBy(h => h.HelpedAt))
            .ThenInclude(x => x.Helper)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((CurrentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var culture = CultureInfo.CurrentUICulture.Name;

        Posts = posts.Select(x => new WallPost(
                x.Id,
                x.Player!.Username,
                x.Player.ProfilePictureUrl ?? "/blank.png",
                x.TargetPlayer?.Username,
                feedTextRenderer.Render(x, x.Player.Username, culture),
                x.UserMessage,
                x.CreatedAt,
                x.ExpiresAt <= now,
                x.Helpers.Count,
                x.MaxHelpers,
                x.CanHelp(player.Id, now),
                x.Helpers.Any(h => h.Helper!.Id == player.Id),
                x.Helpers.Select(h => h.Helper!.Username).ToList()))
            .ToList();

        if (Tab == MineTab)
        {
            player.MarkSocialNotificationsSeen(now);
            await dbContext.SaveChangesAsync(ct);
        }

        ViewData["PlayerName"] = player.Username;
        ViewData["PlayerLevel"] = player.Level;
        ViewData["ProfilePictureUrl"] = player.ProfilePictureUrl ?? "/blank.png";

        return Page();
    }

    public async Task<IActionResult> OnPostHelpAsync(int postId, string? tab, int pageNumber = 1, CancellationToken ct = default)
    {
        var appUser = await userManager.GetUserAsync(User);

        if (appUser is null)
        {
            return RedirectToPage("/Account/Login");
        }

        var helper = await dbContext.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.InventoryItems)
            .Include(x => x.Cars)
            .Include(x => x.CraftingParts)
            .Include(x => x.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .FirstOrDefaultAsync(x => x.AppUser!.Id == appUser.Id, ct);

        var post = await dbContext.Set<SocialPost>()
            .AsSplitQuery()
            .Include(x => x.Helpers)
            .ThenInclude(x => x.Helper)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Cars)
            .Include(x => x.Player)
            .ThenInclude(x => x!.CraftingParts)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Collections)
            .ThenInclude(x => x.Items)
            .Include(x => x.Player)
            .ThenInclude(x => x!.Quests.Where(q => q.QuestType == QuestType.Active))
            .Include(x => x.Player)
            .ThenInclude(x => x!.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects)
            .FirstOrDefaultAsync(x => x.Id == postId, ct);

        var now = ServerUtils.TimeProvider.GetUtcNow().UtcDateTime;

        if (helper is null || post is null || !post.CanHelp(helper.Id, now))
        {
            TempData["Error"] = localizer["WallHelpError"].Value;

            return RedirectToPage("/Social/Wall", new { tab, pageNumber });
        }

        var rewards = post.Help(helper, now);

        try
        {
            await dbContext.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = localizer["WallHelpError"].Value;

            return RedirectToPage("/Social/Wall", new { tab, pageNumber });
        }

        var received = string.Join(", ", rewards.Select(x => x.ItemName is null ? $"{x.Amount} {localizer[x.Type].Value}" : $"{x.Amount} {x.ItemName}"));

        TempData["Success"] = string.IsNullOrEmpty(received) ? localizer["WallHelped"].Value : localizer["WallRewardReceived", received].Value;

        return RedirectToPage("/Social/Wall", new { tab, pageNumber });
    }

    private async Task<Player?> GetCurrentPlayerAsync(CancellationToken ct)
    {
        var appUser = await userManager.GetUserAsync(User);

        if (appUser is null)
        {
            return null;
        }

        return await dbContext.Set<Player>().FirstOrDefaultAsync(x => x.AppUser!.Id == appUser.Id, ct);
    }

    private async Task<List<Guid>> GetAcceptedFriendIdsAsync(Guid playerId, CancellationToken ct)
    {
        return await dbContext.Set<Friend>()
            .Where(x => x.Player!.Id == playerId && x.Status == FriendshipStatus.Accepted)
            .Select(x => x.FriendPlayer!.Id)
            .ToListAsync(ct);
    }

    private async Task<bool> HasUnseenHelpsAsync(Player player, CancellationToken ct)
    {
        return await dbContext.Set<SocialPost>()
            .Where(x => x.Player!.Id == player.Id)
            .SelectMany(x => x.Helpers)
            .AnyAsync(x => x.HelpedAt > player.SocialNotificationsSeenAt, ct);
    }
}

public record WallPost(
    int Id,
    string AuthorName,
    string AuthorPicture,
    string? TargetName,
    FeedText Text,
    string? UserMessage,
    DateTime CreatedAt,
    bool Expired,
    int HelpersCount,
    int MaxHelpers,
    bool CanHelp,
    bool AlreadyHelped,
    List<string> HelperNames);