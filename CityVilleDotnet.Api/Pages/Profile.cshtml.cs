using System.Text.RegularExpressions;
using SkiaSharp;
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

namespace CityVilleDotnet.Api.Pages;

[Authorize]
public class ProfileModel(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext, IWebHostEnvironment webHost, IStringLocalizer<Resources.SharedResource> localizer) : PageModel
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
    public string? ProfilePictureUrl { get; set; }

    public string WorldName { get; set; } = string.Empty;
    public int Population { get; set; }
    public int PopulationCap { get; set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }

    public int CurrentLevelXp { get; set; }
    public int NextLevelXp { get; set; }
    public int XpProgressPercent { get; set; }
    public bool IsMaxLevel { get; set; }

    [BindProperty]
    public IFormFile? PictureUpload { get; set; }

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    private static readonly HashSet<string> AllowedExtensions = [".png", ".jpg", ".jpeg"];
    private const int MaxFileSizeMb = 2;
    private const int MaxDimension = 50;

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        var result = await LoadProfileAsync(ct);
        
        return result;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        var result = await LoadProfileAsync(ct);

        if (PictureUpload is null || PictureUpload.Length == 0)
        {
            ErrorMessage = localizer["SelectFile"];
            return result;
        }

        if (PictureUpload.Length > MaxFileSizeMb * 1024 * 1024)
        {
            ErrorMessage = localizer["FileTooLarge", MaxFileSizeMb];
            return result;
        }

        var ext = Path.GetExtension(PictureUpload.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(ext))
        {
            ErrorMessage = localizer["OnlyPngJpg"];
            return result;
        }

        var picturesDir = Path.Combine(webHost.WebRootPath, "pictures");
        Directory.CreateDirectory(picturesDir);

        var baseName = Regex.Replace(UserName, @"[^a-zA-Z0-9_-]", "_");
        foreach (var oldFile in Directory.EnumerateFiles(picturesDir, $"{baseName}.*"))
        {
            System.IO.File.Delete(oldFile);
        }

        await using var uploadStream = PictureUpload.OpenReadStream();

        using var original = SKBitmap.Decode(uploadStream);

        var savePath = Path.Combine(picturesDir, $"{baseName}{ext}");

        if (original.Width != MaxDimension || original.Height != MaxDimension)
        {
            using var resized = new SKBitmap(MaxDimension, MaxDimension);
            using var canvas = new SKCanvas(resized);
            
            canvas.DrawBitmap(original, new SKRect(0, 0, MaxDimension, MaxDimension), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
            
            using var image = SKImage.FromBitmap(resized);
            using var data = ext is ".png" ? image.Encode(SKEncodedImageFormat.Png, 100) : image.Encode(SKEncodedImageFormat.Jpeg, 90);
            
            await using var fs = new FileStream(savePath, FileMode.Create);
            
            data.SaveTo(fs);
        }

        var currentUser = await userManager.GetUserAsync(User);

        var player = await dbContext.Set<Player>().FirstOrDefaultAsync(x => x.AppUser!.Id == currentUser!.Id, ct);

        if (player is null)
            return RedirectToPage("/Account/Login");

        var now = ServerUtils.GetCurrentTime();
        
        player.ProfilePictureUrl = $"/pictures/{baseName}{ext}?version={now}";

        await dbContext.SaveChangesAsync(ct);

        ProfilePictureUrl = player.ProfilePictureUrl;
        SuccessMessage = localizer["PictureUpdated"];

        ViewData["ProfilePictureUrl"] = ProfilePictureUrl;

        return result;
    }

    private async Task<IActionResult> LoadProfileAsync(CancellationToken ct)
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
        ProfilePictureUrl = player.ProfilePictureUrl;

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
        ViewData["ProfilePictureUrl"] = ProfilePictureUrl ?? "/blank.png";

        return Page();
    }
}
