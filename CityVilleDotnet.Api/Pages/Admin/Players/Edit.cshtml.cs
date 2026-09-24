using System.ComponentModel.DataAnnotations;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CityVilleDotnet.Api.Pages.Admin.Players;

public class EditModel(CityVilleDbContext dbContext, UserManager<ApplicationUser> userManager, IStringLocalizer<Resources.SharedResource> localizer) : PageModel
{
    private const int MaxItemNameLength = 64;
    private const int MaxQuestNameLength = 64;
    private const int MaxCouponNameLength = 64;

    public Player Player { get; set; } = null!;
    public List<WorldRow> Worlds { get; set; } = [];
    public bool IsOnline { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        var player = await dbContext.Set<Player>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.AppUser)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests)
            .Include(x => x.Coupons)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (player is null)
            return NotFound();

        Player = player;
        IsOnline = player.LastTrackingTimestamp >= DateTimeOffset.Now - Player.OnlineThreshold;

        Worlds = await dbContext.Set<World>()
            .AsNoTracking()
            .Where(x => x.Player!.Id == id)
            .OrderBy(x => x.Type)
            .Select(x => new WorldRow(x.Type, x.WorldName, x.Objects.Count, x.Population, x.PopulationCap, x.SizeX, x.SizeY))
            .ToListAsync(ct);

        return Page();
    }

    public async Task<IActionResult> OnPostUpdatePlayerAsync(Guid id, PlayerInput input, CancellationToken ct)
    {
        var maxLevel = GameSettingsManager.Instance.GetLevels().Max(x => x.Num);
        var maxSocialLevel = GameSettingsManager.Instance.GetSocialLevels().Max(x => x.Num);

        if (!ModelState.IsValid || input.Level > maxLevel || input.SocialLevel > maxSocialLevel)
        {
            TempData["Error"] = localizer["AdminInvalidValues"].Value;
            return RedirectToTab(id, "overview");
        }

        var player = await dbContext.Set<Player>().FirstOrDefaultAsync(x => x.Id == id, ct);

        if (player is null)
            return NotFound();

        player.SetGold(input.Gold);
        player.SetCash(input.Cash);
        player.SetGoods(input.Goods);
        player.SetPremiumGoods(input.PremiumGoods);
        player.UpdateProgression(input.Level, input.Xp);
        player.UpdateSocialProgression(input.SocialLevel, input.SocialXp);
        player.SetEnergy(input.Energy);

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminPlayerUpdated"].Value;
        return RedirectToTab(id, "overview");
    }

    public async Task<IActionResult> OnPostToggleAdminAsync(Guid id, CancellationToken ct)
    {
        var appUser = await dbContext.Set<Player>()
            .Where(x => x.Id == id)
            .Select(x => x.AppUser)
            .FirstOrDefaultAsync(ct);

        if (appUser is null)
            return NotFound();

        if (appUser.Id == userManager.GetUserId(User))
        {
            TempData["Error"] = localizer["AdminCannotRemoveOwnAdmin"].Value;
            return RedirectToTab(id, "overview");
        }

        appUser.SetAdmin(!appUser.IsAdmin);

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminPlayerUpdated"].Value;
        return RedirectToTab(id, "overview");
    }

    public async Task<IActionResult> OnPostAddItemAsync(Guid id, string? itemName, int amount, CancellationToken ct)
    {
        itemName = itemName?.Trim();

        if (string.IsNullOrEmpty(itemName) || itemName.Length > MaxItemNameLength || amount <= 0 || GameSettingsManager.Instance.GetItem(itemName) is null)
        {
            TempData["Error"] = localizer["AdminUnknownItem"].Value;
            return RedirectToTab(id, "inventory");
        }

        var player = await dbContext.Set<Player>()
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (player is null)
            return NotFound();

        player.AddItem(itemName, amount);

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminInventoryUpdated"].Value;
        return RedirectToTab(id, "inventory");
    }

    public async Task<IActionResult> OnPostUpdateItemAsync(Guid id, int itemId, int amount, CancellationToken ct)
    {
        if (amount <= 0)
        {
            TempData["Error"] = localizer["AdminInvalidValues"].Value;
            return RedirectToTab(id, "inventory");
        }

        var item = await dbContext.Set<Player>()
            .Where(x => x.Id == id)
            .SelectMany(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == itemId, ct);

        if (item is null)
            return NotFound();

        item.SetAmount(amount);

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminInventoryUpdated"].Value;
        return RedirectToTab(id, "inventory");
    }

    public async Task<IActionResult> OnPostDeleteItemAsync(Guid id, int itemId, CancellationToken ct)
    {
        var item = await dbContext.Set<Player>()
            .Where(x => x.Id == id)
            .SelectMany(x => x.InventoryItems)
            .Include(x => x.StoredObject)
            .FirstOrDefaultAsync(x => x.Id == itemId, ct);

        if (item is null)
            return NotFound();

        dbContext.Remove(item);

        if (item.StoredObject is not null)
            dbContext.Remove(item.StoredObject);

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminInventoryUpdated"].Value;
        return RedirectToTab(id, "inventory");
    }

    public async Task<IActionResult> OnPostAddCouponAsync(Guid id, string? couponName, int? worldFlatId, CancellationToken ct)
    {
        couponName = couponName?.Trim();

        if (string.IsNullOrEmpty(couponName) || couponName.Length > MaxCouponNameLength || worldFlatId <= 0)
        {
            TempData["Error"] = localizer["AdminInvalidValues"].Value;
            return RedirectToTab(id, "coupons");
        }

        var player = await dbContext.Set<Player>()
            .Include(x => x.Coupons)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (player is null)
            return NotFound();

        if (!player.GiveCoupon(couponName, worldFlatId))
        {
            TempData["Error"] = localizer["AdminCouponAlreadyOwned"].Value;
            return RedirectToTab(id, "coupons");
        }

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminCouponsUpdated"].Value;
        return RedirectToTab(id, "coupons");
    }

    public async Task<IActionResult> OnPostDeleteCouponAsync(Guid id, int couponId, CancellationToken ct)
    {
        var coupon = await dbContext.Set<Player>()
            .Where(x => x.Id == id)
            .SelectMany(x => x.Coupons)
            .FirstOrDefaultAsync(x => x.Id == couponId, ct);

        if (coupon is null)
            return NotFound();

        dbContext.Remove(coupon);

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminCouponsUpdated"].Value;
        return RedirectToTab(id, "coupons");
    }

    public async Task<IActionResult> OnPostStartQuestAsync(Guid id, string? questName, CancellationToken ct)
    {
        questName = questName?.Trim();

        var questItem = string.IsNullOrEmpty(questName) || questName.Length > MaxQuestNameLength
            ? null
            : QuestSettingsManager.Instance.GetItem(questName);

        if (questItem is null)
        {
            TempData["Error"] = localizer["AdminUnknownQuest"].Value;
            return RedirectToTab(id, "quests");
        }

        var player = await dbContext.Set<Player>()
            .Include(x => x.Quests)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (player is null)
            return NotFound();

        if (!player.StartQuest(questItem))
        {
            TempData["Error"] = localizer["AdminQuestAlreadyStarted"].Value;
            return RedirectToTab(id, "quests");
        }

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminQuestUpdated"].Value;
        return RedirectToTab(id, "quests");
    }

    public async Task<IActionResult> OnPostUpdateQuestAsync(Guid id, int questId, QuestType questType, CancellationToken ct)
    {
        if (!Enum.IsDefined(questType))
        {
            TempData["Error"] = localizer["AdminInvalidValues"].Value;
            return RedirectToTab(id, "quests");
        }

        var quest = await FindQuestAsync(id, questId, ct);

        if (quest is null)
            return NotFound();

        quest.SetQuestType(questType);

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminQuestUpdated"].Value;
        return RedirectToTab(id, "quests");
    }

    public async Task<IActionResult> OnPostDeleteQuestAsync(Guid id, int questId, CancellationToken ct)
    {
        var quest = await FindQuestAsync(id, questId, ct);

        if (quest is null)
            return NotFound();

        dbContext.Remove(quest);

        await dbContext.SaveChangesAsync(ct);

        TempData["Success"] = localizer["AdminQuestUpdated"].Value;
        return RedirectToTab(id, "quests");
    }

    private Task<Quest?> FindQuestAsync(Guid id, int questId, CancellationToken ct)
    {
        return dbContext.Set<Player>()
            .Where(x => x.Id == id)
            .SelectMany(x => x.Quests)
            .FirstOrDefaultAsync(x => x.Id == questId, ct);
    }

    private RedirectToPageResult RedirectToTab(Guid id, string tab)
    {
        return RedirectToPage("/Admin/Players/Edit", null, new { id }, tab);
    }

    public record WorldRow(WorldType Type, string WorldName, int ObjectsCount, int Population, int PopulationCap, int SizeX, int SizeY);

    public class PlayerInput
    {
        [Range(0, int.MaxValue)] public int Gold { get; set; }
        [Range(0, int.MaxValue)] public int Cash { get; set; }
        [Range(0, int.MaxValue)] public int Goods { get; set; }
        [Range(0, int.MaxValue)] public int PremiumGoods { get; set; }
        [Range(1, int.MaxValue)] public int Level { get; set; }
        [Range(0, int.MaxValue)] public int Xp { get; set; }
        [Range(0, int.MaxValue)] public int Energy { get; set; }
        [Range(1, int.MaxValue)] public int SocialLevel { get; set; }
        [Range(0, int.MaxValue)] public int SocialXp { get; set; }
    }
}
