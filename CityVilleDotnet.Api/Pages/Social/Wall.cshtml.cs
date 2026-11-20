using System.Globalization;
using CityVilleDotnet.Api.Common.Social;
using CityVilleDotnet.Common.Settings;
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
    public const string ReceivedTab = "received";

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

        if (tab is CommunityTab or MineTab or FriendsTab or ReceivedTab)
        {
            Tab = tab;
        }
        else if (await HasPendingReceivedAsync(player, now, ct))
        {
            Tab = ReceivedTab;
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
            ReceivedTab => query.Where(x => x.Type != SocialPostType.Feed && x.TargetPlayer!.Id == player.Id && x.ExpiresAt > now && !x.Helpers.Any()),
            CommunityTab => query.Where(x => x.Type == SocialPostType.Feed && x.ExpiresAt > now && x.Player!.Id != player.Id && !friendIds.Contains(x.Player.Id)),
            _ => query.Where(x => x.Type == SocialPostType.Feed && x.ExpiresAt > now && friendIds.Contains(x.Player!.Id))
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
                x.Type,
                x.Type == SocialPostType.Feed ? feedTextRenderer.Render(x, x.Player.Username, culture) : RenderRequest(x, x.Player.Id == player.Id, culture),
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
            .Include(x => x.TargetPlayer)
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

        var culture = CultureInfo.CurrentUICulture.Name;
        var received = string.Join(", ", rewards.Select(x => x.ItemName is null ? $"{x.Amount} {localizer[x.Type].Value}" : $"{x.Amount} {feedTextRenderer.ItemName(x.ItemName, culture)}"));

        TempData["Success"] = post.Type switch
        {
            SocialPostType.Gift => localizer["WallGiftAccepted", received].Value,
            SocialPostType.Request => localizer["WallRequestGiven", feedTextRenderer.ItemName(post.Key, culture), post.Player!.Username].Value,
            _ => string.IsNullOrEmpty(received) ? localizer["WallHelped"].Value : localizer["WallRewardReceived", received].Value
        };

        return RedirectToPage("/Social/Wall", new { tab, pageNumber });
    }

    private FeedText RenderRequest(SocialPost post, bool isMine, string culture)
    {
        var textKey = (post.Type, isMine) switch
        {
            (SocialPostType.Gift, true) => "WallGiftSentText",
            (SocialPostType.Gift, false) => "WallGiftText",
            (_, true) => "WallRequestSentText",
            _ => "WallRequestText"
        };
        var name = isMine ? post.TargetPlayer!.Username : post.Player!.Username;
        var status = isMine && post.Helpers.Count > 0 ? localizer["WallRequestDone"].Value : null;
        var icon = GameSettingsManager.Instance.GetItem(post.Key)?.Images.FirstOrDefault(x => x.Name == "icon")?.Url;

        return new FeedText(localizer[textKey, name, feedTextRenderer.ItemName(post.Key, culture)].Value, status, icon is null ? null : "/" + icon);
    }

    private async Task<bool> HasPendingReceivedAsync(Player player, DateTime now, CancellationToken ct)
    {
        return await dbContext.Set<SocialPost>()
            .AnyAsync(x => x.Type != SocialPostType.Feed && x.TargetPlayer!.Id == player.Id && x.ExpiresAt > now && !x.Helpers.Any(), ct);
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
    SocialPostType Type,
    FeedText Text,
    string? UserMessage,
    DateTime CreatedAt,
    bool Expired,
    int HelpersCount,
    int MaxHelpers,
    bool CanHelp,
    bool AlreadyHelped,
    List<string> HelperNames);