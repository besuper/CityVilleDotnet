using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using CityVilleDotnet.Common.Utils;

namespace CityVilleDotnet.Api.Pages;

[Authorize]
public class GameModel(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext) : PageModel
{
    public string FriendList { get; set; } = "[]";
    public string Uid { get; set; } = "333";
    public string UserName { get; set; } = "Steve";
    public int Level { get; set; } = 1;
    public long ServerTime { get; set; } = 0;

    public async Task<IActionResult> OnGetAsync()
    {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser is null)
            return RedirectToPage("/Account/Login");

        var user = await dbContext.Set<User>()
            .AsNoTracking()
            .Include(x => x.AppUser)
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.AppUser!.Id.Equals(currentUser.Id));

        ServerTime = ServerUtils.GetCurrentTime();

        if (user?.Player is not null)
        {
            Uid = user.Player.Uid;
            UserName = user.Player.Username;
            Level = user.Player.Level;
            FriendList = JsonSerializer.Serialize(user.GetFriendsData(), new JsonSerializerOptions { WriteIndented = false, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        }
        else
        {
            UserName = currentUser.UserName ?? "Unknown";
        }

        return Page();
    }

    public string BuildFlashVars()
    {
        var flashVars = new Dictionary<string, string>()
        {
            ["serverTime"] = $"{ServerTime}",
            ["swfLocation"] = "Game.2012.swf",
            ["zySnid"] = "0",
            ["zySnuid"] = $"{Uid}",
            ["zyUid"] = $"{Uid}",
            ["zyAuthHash"] = $"{Uid}",
            ["zySig"] = $"{Uid}",
            ["zcache_gameswf_gamesettings"] = "false",
            ["static_asset_prefix"] = "",
            ["app_fb_proxy_url"] = "",
            ["flashRevision"] = "26346",
            ["generateSchema"] = "1",
            ["sn_app_url"] = "",
            ["snapiEnable"] = "1",
            ["game_config_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/gameSettings.xml",
            ["quest_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/questSettings.xml",
            ["effects_config_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/effectsConfig.xml",
            ["font_mapper_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/FontMapper.swf",
            ["localization_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/lang/locale_en_US.swf",
            ["bootstrap_config_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/bootstrap.xml",
            ["amf_settings_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/settings.amf.z",
            ["embedded_art_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/EmbeddedArt.swf",
            ["static_asset_urls"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/",
            ["asset_urls"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/",
            ["app_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/",
            ["asset_url"] = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/assets/",
            ["preloaded_asset_urls"] =
                $"{Request.Scheme}://{Request.Host}{Request.PathBase}/assets/road/city/city04_SE.png,{Request.Scheme}://{Request.Host}{Request.PathBase}/assets/road/city/city04_SW.png,{Request.Scheme}://{Request.Host}{Request.PathBase}/assets/dialogs/MarketAssets.swf",
        };

        foreach (var param in Request.Query)
        {
            flashVars[param.Key] = param.Value.ToString();
        }

        return string.Join("&", flashVars.Select(kvp => $"{kvp.Key}={kvp.Value}"));
    }
}