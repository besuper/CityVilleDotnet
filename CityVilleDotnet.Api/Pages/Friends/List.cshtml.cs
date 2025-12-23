using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Pages.Friends;

[Authorize]
public class ListModel(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext) : PageModel
{
    public ApplicationUser? CurrentUser { get; set; }
    public List<FriendDto> Friends { get; set; } = [];

    [BindProperty] public string? Username { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        var user = await GetCurrentUserAsync(ct);

        if (user is null)
            return RedirectToPage("/Account/Login");

        CurrentUser = user.AppUser;

        Friends = await dbContext.Set<Friend>()
            .AsNoTracking()
            .Where(x => x.User.AppUser!.Id == CurrentUser.Id)
            .Include(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .Select(x => x.ToDto())
            .OrderBy(x => x.Level)
            .ToListAsync(ct);

        return Page();
    }

    public async Task<IActionResult> OnPostAddFriend(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            TempData["Error"] = "Username cannot be empty.";
            return RedirectToPage("/Friends/List");
        }

        var user = await GetCurrentUserAsync(ct);

        if (user?.Player is null)
            return RedirectToPage("/Account/Login");

        CurrentUser = user.AppUser;

        if (user.Player.Username.Equals(Username, StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = "You cannot add yourself as a friend.";
            return RedirectToPage("/Friends/List");
        }

        var targetUser = await dbContext.Set<User>()
            .Include(x => x.Player)
            .Where(x => x.Player!.Username == Username)
            .FirstOrDefaultAsync(ct);

        if (targetUser is null)
        {
            TempData["Error"] = "User not found.";
            return RedirectToPage("/Friends/List");
        }

        var existingFriendship = await dbContext.Set<Friend>()
            .AnyAsync(x => x.User.Id == user.Id && x.FriendUser.Id == targetUser.Id, ct);

        if (existingFriendship)
        {
            TempData["Error"] = "This user is already in your friend list.";
            return RedirectToPage("/Friends/List");
        }

        var friendship1 = new Friend(targetUser, user, true);
        var friendship2 = new Friend(user, targetUser, false);

        targetUser.Friends.Add(friendship1);
        user.Friends.Add(friendship2);

        TempData["Success"] = $"Friend request sent to {Username}.";

        await dbContext.SaveChangesAsync(ct);

        return RedirectToPage("/Friends/List");
    }

    public async Task<IActionResult> OnGetAccept(string userName, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            TempData["Error"] = "Invalid username.";
            return RedirectToPage("/Friends/List");
        }

        var user = await GetCurrentUserAsync(ct);

        if (user is null)
            return RedirectToPage("/Account/Login");

        CurrentUser = user.AppUser;

        var friendship = await dbContext.Set<Friend>()
            .Include(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(x => x.User.Id == user.Id && x.FriendUser.Player!.Username == userName, ct);

        if (friendship is null)
        {
            TempData["Error"] = "Friend request not found.";
            return RedirectToPage("/Friends/List");
        }

        var targetFriendship = await dbContext.Set<Friend>()
            .FirstOrDefaultAsync(x => x.User.Id == friendship.FriendUser.Id && x.FriendUser.Id == user.Id, ct);

        if (targetFriendship is null)
        {
            TempData["Error"] = "Error accepting friend request.";
            return RedirectToPage("/Friends/List");
        }

        friendship.Status = FriendshipStatus.Accepted;
        targetFriendship.Status = FriendshipStatus.Accepted;

        TempData["Success"] = $"You are now friends with {userName}.";

        await dbContext.SaveChangesAsync(ct);

        return RedirectToPage("/Friends/List");
    }

    public async Task<IActionResult> OnGetReject(string userName, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            TempData["Error"] = "Invalid username.";
            return RedirectToPage("/Friends/List");
        }

        var user = await GetCurrentUserAsync(ct);

        if (user is null)
            return RedirectToPage("/Account/Login");

        CurrentUser = user.AppUser;

        var friendship = await dbContext.Set<Friend>()
            .Include(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(x => x.User.Id == user.Id && x.FriendUser.Player!.Username == userName, ct);

        if (friendship is null)
        {
            TempData["Error"] = "Friend request not found.";
            return RedirectToPage("/Friends/List");
        }

        var targetFriendship = await dbContext.Set<Friend>()
            .FirstOrDefaultAsync(x => x.User.Id == friendship.FriendUser.Id && x.FriendUser.Id == user.Id, ct);

        dbContext.Set<Friend>().Remove(friendship);

        if (targetFriendship is not null)
        {
            dbContext.Set<Friend>().Remove(targetFriendship);
        }

        TempData["Success"] = $"Friend request from {userName} rejected.";

        await dbContext.SaveChangesAsync(ct);
        return RedirectToPage("/Friends/List");
    }

    public async Task<IActionResult> OnGetCancel(string userName, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            TempData["Error"] = "Invalid username.";
            return RedirectToPage("/Friends/List");
        }

        var user = await GetCurrentUserAsync(ct);

        if (user is null)
            return RedirectToPage("/Account/Login");

        CurrentUser = user.AppUser;

        var friendship = await dbContext.Set<Friend>()
            .Include(x => x.FriendUser)
            .ThenInclude(x => x.Player)
            .FirstOrDefaultAsync(x => x.User.Id == user.Id && x.FriendUser.Player!.Username == userName, ct);

        if (friendship is null)
        {
            TempData["Error"] = "Friend request not found.";
            return RedirectToPage("/Friends/List");
        }

        var targetFriendship = await dbContext.Set<Friend>()
            .FirstOrDefaultAsync(x => x.User.Id == friendship.FriendUser.Id && x.FriendUser.Id == user.Id, ct);

        dbContext.Set<Friend>().Remove(friendship);

        if (targetFriendship is not null) dbContext.Set<Friend>().Remove(targetFriendship);

        TempData["Success"] = $"Friend request to {userName} cancelled.";

        await dbContext.SaveChangesAsync(ct);
        return RedirectToPage("/Friends/List");
    }

    private async Task<User?> GetCurrentUserAsync(CancellationToken ct)
    {
        CurrentUser = await userManager.GetUserAsync(User);

        if (CurrentUser is null)
            return null;

        return await dbContext.Set<User>()
            .Include(x => x.AppUser)
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.AppUser!.Id == CurrentUser.Id, ct);
    }
}