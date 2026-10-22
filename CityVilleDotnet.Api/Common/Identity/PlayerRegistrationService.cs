using System.Text.Json;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CityVilleDotnet.Api.Common.Identity;

public class PlayerRegistrationService(
    UserManager<ApplicationUser> userManager,
    CityVilleDbContext context,
    IStringLocalizer<Resources.SharedResource> localizer)
{
    public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string? password = null)
    {
        IdentityResult result;

        try
        {
            result = password is null ? await userManager.CreateAsync(user) : await userManager.CreateAsync(user, password);
        }
        catch (Exception)
        {
            return IdentityResult.Failed(new IdentityError { Description = localizer["UsernameTaken"] });
        }

        if (!result.Succeeded) return result;

        var jsonContent = await File.ReadAllTextAsync("Resources/startWorld.json");
        var defaultWorld = JsonSerializer.Deserialize<WorldDto>(jsonContent) ?? throw new Exception("WorldDto can't be null");

        var mapRects = defaultWorld.MapRects.Select(x => new MapRect()
        {
            Height = x.Height,
            Width = x.Width,
            X = x.X,
            Y = x.Y,
        }).ToList();

        var objects = defaultWorld.Objects.Select(x => new WorldObject().LoadObject(x)).ToList();

        var world = new World("", 36, 36, 30, 0, 50, 0, 0, mapRects, objects);

        var newPlayer = new Player(user, world);

        await context.AddAsync(newPlayer);

        var samantha = await context.Set<Player>()
            .Include(u => u.Friends)
            .FirstOrDefaultAsync(u => u.Snuid == -1);

        if (samantha is not null)
        {
            var friendship1 = new Friend(samantha, newPlayer, true) { Status = FriendshipStatus.Accepted };
            var friendship2 = new Friend(newPlayer, samantha, false) { Status = FriendshipStatus.Accepted };

            samantha.Friends.Add(friendship1);
            newPlayer.Friends.Add(friendship2);
        }

        await context.SaveChangesAsync();

        return result;
    }
}