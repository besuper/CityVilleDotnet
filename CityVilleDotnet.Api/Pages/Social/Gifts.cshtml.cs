using System.Globalization;
using System.Text.Json;
using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Api.Common.Social;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CityVilleDotnet.Api.Pages.Social;

[Authorize]
public class GiftsModel(CityVilleDbContext dbContext, FeedTextRenderer feedTextRenderer, IStringLocalizer<Resources.SharedResource> localizer) : PageModel
{
    public List<GiftOption> Gifts { get; private set; } = [];
    public List<GiftOption> Materials { get; private set; } = [];
    public List<GiftRecipient> Recipients { get; private set; } = [];
    public string? SelectedGift { get; private set; }
    public int? SelectedRecipient { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? gift, string? giftRecipient, CancellationToken ct)
    {
        var playerId = User.GetPlayerId();
        var now = ServerUtils.TimeProvider.GetUtcNow().UtcDateTime;
        var giftsSince = now.AddDays(-1);

        var player = await dbContext.Set<Player>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendPlayer)
            .Include(x => x.SocialPosts.Where(p => p.Type == SocialPostType.Gift && p.CreatedAt > giftsSince))
            .ThenInclude(x => x.TargetPlayer)
            .FirstOrDefaultAsync(x => x.Id == playerId, ct);

        if (player is null)
        {
            return RedirectToPage("/Account/Login");
        }

        var culture = CultureInfo.CurrentUICulture.Name;
        var options = GameSettingsManager.Instance.GetGiftableItems()
            .Select(x => new GiftOption(x.Name, feedTextRenderer.ItemName(x.Name, culture), IconUrl(x.Images.FirstOrDefault(i => i.Name == "icon")?.Url)))
            .ToList();

        Gifts = options.Where(x => !x.Name.StartsWith("material_")).OrderBy(x => x.DisplayName).ToList();
        Materials = options.Where(x => x.Name.StartsWith("material_")).OrderBy(x => x.DisplayName).ToList();

        var giftedSnuids = player.GetSentGifts(now).Select(x => x.RecipientSnuid).ToHashSet();

        Recipients = player.Friends
            .Select(x => x.GetFriend())
            .Where(x => !x.IsSamantha())
            .OrderBy(x => x.Username)
            .Select(x => new GiftRecipient(x.Snuid, x.Username, x.ProfilePictureUrl ?? "/blank.png", giftedSnuids.Contains(x.Snuid)))
            .ToList();

        SelectedGift = options.Any(x => x.Name == gift) ? gift : null;
        SelectedRecipient = int.TryParse(giftRecipient, out var recipient) ? recipient : null;

        ViewData["PlayerName"] = player.Username;
        ViewData["PlayerLevel"] = player.Level;
        ViewData["ProfilePictureUrl"] = player.ProfilePictureUrl ?? "/blank.png";

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? gift, List<int> recipients, CancellationToken ct)
    {
        if (gift is null || GameSettingsManager.Instance.GetGiftableItems().All(x => x.Name != gift))
        {
            TempData["Error"] = localizer["GiftsChooseGift"].Value;
            return RedirectToPage(new { gift });
        }

        var playerId = User.GetPlayerId();

        var player = await dbContext.Set<Player>()
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendPlayer)
            .FirstOrDefaultAsync(x => x.Id == playerId, ct);

        if (player is null)
        {
            return RedirectToPage("/Account/Login");
        }

        var data = JsonSerializer.Serialize(new { item = gift });
        var sent = player.SendRequest(SocialPostType.Gift, gift, data, recipients, ServerUtils.TimeProvider.GetUtcNow().UtcDateTime);

        if (sent.Count == 0)
        {
            TempData["Error"] = localizer["GiftsChooseFriends"].Value;
            return RedirectToPage(new { gift });
        }

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["GiftsSent", feedTextRenderer.ItemName(gift, CultureInfo.CurrentUICulture.Name), string.Join(", ", sent.Select(x => x.Username))].Value;

        return RedirectToPage(new { gift });
    }

    private static string? IconUrl(string? url)
    {
        return url is null ? null : "/" + url;
    }
}

public record GiftOption(string Name, string DisplayName, string? IconUrl);

public record GiftRecipient(int Snuid, string Name, string Picture, bool AlreadyGifted);