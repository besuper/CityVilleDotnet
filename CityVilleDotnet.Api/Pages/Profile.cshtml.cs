using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Pages;

[Authorize]
public class ProfileModel(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext) : PageModel
{
    public string UserName { get; set; } = string.Empty;
    public int Level { get; set; }
    public int Xp { get; set; }
    public int Gold { get; set; }
    public int Cash { get; set; }
    public int Goods { get; set; }
    public int PremiumGoods { get; set; }
    public int Energy { get; set; }
    public int EnergyMax { get; set; }
    public int SocialLevel { get; set; }
    public int SocialXp { get; set; }
    public int ExpansionsPurchased { get; set; }
    public DateTimeOffset CreationTimestamp { get; set; }
    public int FriendsCount { get; set; }

    public string WorldName { get; set; } = string.Empty;
    public int Population { get; set; }
    public int PopulationCap { get; set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }

    public int CurrentLevelXp { get; set; }
    public int NextLevelXp { get; set; }
    public int XpProgressPercent { get; set; }
    public bool IsMaxLevel { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser is null)
            return RedirectToPage("/Account/Login");

        var player = await dbContext.Set<Player>()
            .AsNoTracking()
            .Include(x => x.Worlds.Where(w => w.Type == WorldType.Main))
            .FirstOrDefaultAsync(x => x.AppUser!.Id == currentUser.Id, ct);

        if (player is null)
            return RedirectToPage("/Account/Login");

        FriendsCount = await dbContext.Set<Player>()
            .Where(x => x.AppUser!.Id == currentUser.Id)
            .SelectMany(x => x.Friends)
            .CountAsync(x => x.Status == FriendshipStatus.Accepted && x.FriendPlayer!.Snuid != -1, ct);

        UserName = player.Username;
        Level = player.Level;
        Xp = player.Xp;
        Gold = player.Gold;
        Cash = player.Cash;
        Goods = player.Goods;
        PremiumGoods = player.PremiumGoods;
        Energy = player.Energy;
        EnergyMax = player.EnergyMax;
        SocialLevel = player.SocialLevel;
        SocialXp = player.SocialXp;
        ExpansionsPurchased = player.ExpansionsPurchased;
        CreationTimestamp = player.CreationTimestamp;

        var mainWorld = player.GetWorldByType(WorldType.Main);

        if (mainWorld is not null)
        {
            WorldName = mainWorld.WorldName;
            Population = mainWorld.GetCurrentPopulation();
            PopulationCap = mainWorld.PopulationCap;
            SizeX = mainWorld.SizeX;
            SizeY = mainWorld.SizeY;
        }

        var levels = GameSettingsManager.Instance.GetLevels();
        var currentLevel = levels.FirstOrDefault(x => x.Num == Level);
        var nextLevel = levels.FirstOrDefault(x => x.Num == Level + 1);

        CurrentLevelXp = currentLevel?.RequiredXp ?? 0;

        if (nextLevel is not null && nextLevel.RequiredXp > CurrentLevelXp)
        {
            NextLevelXp = nextLevel.RequiredXp;
            XpProgressPercent = (int)Math.Clamp((double)(Xp - CurrentLevelXp) / (NextLevelXp - CurrentLevelXp) * 100, 0, 100);
        }
        else
        {
            IsMaxLevel = true;
            NextLevelXp = Xp;
            XpProgressPercent = 100;
        }

        ViewData["PlayerName"] = UserName;
        ViewData["PlayerLevel"] = Level;

        return Page();
    }
}
