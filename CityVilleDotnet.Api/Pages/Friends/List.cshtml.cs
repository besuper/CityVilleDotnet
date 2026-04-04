using System.ComponentModel.DataAnnotations;
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

        if (user?.AppUser is null)
            return RedirectToPage("/Account/Login");

        CurrentUser = user.AppUser;

        if (CurrentUser.IsGuest)
        {
            return RedirectToPage("/Game");
        }

        Friends = await dbContext.Set<Player>()
            .AsNoTracking()
            .Where(x => x.AppUser!.Id.Equals(CurrentUser.Id))
            .Include(x => x.AppUser)
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendPlayer)
            .SelectMany(x => x.Friends, (_, friend) => friend)
            .Where(x => x.FriendPlayer!.Snuid != -1) // Remove samantha
            .Select(x => x.ToDto())
            .ToListAsync(ct);

        ViewData["PlayerName"] = user.Username;
        ViewData["PlayerLevel"] = user.Level;

        return Page();
    }

    public async Task<IActionResult> OnPostAddFriend(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            TempData["Error"] = "Username cannot be empty.";
            return RedirectToPage("/Friends/List");
        }

        var player = await GetCurrentUserAsync(ct);

        if (player?.AppUser is null)
            return RedirectToPage("/Account/Login");

        CurrentUser = player.AppUser;

        if (CurrentUser.IsGuest) return RedirectToPage("/Game");

        var targetPlayer = await dbContext.Set<Player>().FirstOrDefaultAsync(x => x.Username == Username, ct);

        if (targetPlayer is null)
        {
            TempData["Error"] = "User not found.";
            return RedirectToPage("/Friends/List");
        }

        if (player.HasFriend(targetPlayer))
        {
            TempData["Error"] = "This user is already in your friend list.";
            return RedirectToPage("/Friends/List");
        }

        player.SendFriendRequest(targetPlayer);

        TempData["Success"] = $"Friend request sent to {Username}.";

        ViewData["PlayerName"] = player.Username;
        ViewData["PlayerLevel"] = player.Level;

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

        if (user?.AppUser is null) return RedirectToPage("/Account/Login");

        CurrentUser = user.AppUser;

        if (CurrentUser.IsGuest) return RedirectToPage("/Game");

        var friendship = await dbContext.Set<Friend>()
            .Include(x => x.Player)
            .Include(x => x.FriendPlayer)
            .FirstOrDefaultAsync(x => x.Player!.Id == user.Id && x.FriendPlayer!.Username == userName, ct);

        if (friendship is null)
        {
            TempData["Error"] = "Friend request not found.";
            return RedirectToPage("/Friends/List");
        }

        var targetFriendship = await dbContext.Set<Friend>()
            .Include(x => x.Player)
            .Include(x => x.FriendPlayer)
            .FirstOrDefaultAsync(x => x.Player!.Id == friendship.FriendPlayer!.Id && x.FriendPlayer!.Id == user.Id, ct);

        if (targetFriendship is null)
        {
            TempData["Error"] = "Error accepting friend request.";
            return RedirectToPage("/Friends/List");
        }

        friendship.Status = FriendshipStatus.Accepted;
        targetFriendship.Status = FriendshipStatus.Accepted;

        TempData["Success"] = $"You are now friends with {userName}.";

        ViewData["PlayerName"] = user.Username;
        ViewData["PlayerLevel"] = user.Level;

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

        if (user?.AppUser is null)
            return RedirectToPage("/Account/Login");

        CurrentUser = user.AppUser;

        if (CurrentUser.IsGuest)
        {
            return RedirectToPage("/Game");
        }

        var friendship = await dbContext.Set<Friend>()
            .Include(x => x.Player)
            .Include(x => x.FriendPlayer)
            .FirstOrDefaultAsync(x => x.Player!.Id == user.Id && x.FriendPlayer!.Username == userName, ct);

        if (friendship is null)
        {
            TempData["Error"] = "Friend request not found.";
            return RedirectToPage("/Friends/List");
        }

        var targetFriendship = await dbContext.Set<Friend>()
            .Include(x => x.Player)
            .Include(x => x.FriendPlayer)
            .FirstOrDefaultAsync(x => x.Player!.Id == friendship.FriendPlayer!.Id && x.FriendPlayer!.Id == user.Id, ct);

        dbContext.Set<Friend>().Remove(friendship);

        if (targetFriendship is not null)
        {
            dbContext.Set<Friend>().Remove(targetFriendship);
        }

        TempData["Success"] = $"Friend request from {userName} rejected.";

        ViewData["PlayerName"] = user.Username;
        ViewData["PlayerLevel"] = user.Level;

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

        if (user?.AppUser is null)
            return RedirectToPage("/Account/Login");

        CurrentUser = user.AppUser;

        if (CurrentUser.IsGuest)
        {
            return RedirectToPage("/Game");
        }

        var friendship = await dbContext.Set<Friend>()
            .Include(x => x.FriendPlayer)
            .FirstOrDefaultAsync(x => x.Player.Id == user.Id && x.FriendPlayer.Username == userName, ct);

        if (friendship is null)
        {
            TempData["Error"] = "Friend request not found.";
            return RedirectToPage("/Friends/List");
        }

        var targetFriendship = await dbContext.Set<Friend>()
            .Include(x => x.Player)
            .FirstOrDefaultAsync(x => x.Player!.Id == friendship.FriendPlayer!.Id && x.FriendPlayer!.Id == user.Id, ct);

        dbContext.Set<Friend>().Remove(friendship);

        if (targetFriendship is not null) dbContext.Set<Friend>().Remove(targetFriendship);

        TempData["Success"] = $"Friend request to {userName} cancelled.";

        ViewData["PlayerName"] = user.Username;
        ViewData["PlayerLevel"] = user.Level;

        await dbContext.SaveChangesAsync(ct);
        return RedirectToPage("/Friends/List");
    }

    private async Task<Player?> GetCurrentUserAsync(CancellationToken ct)
    {
        CurrentUser = await userManager.GetUserAsync(User);

        if (CurrentUser is null)
            return null;

        return await dbContext.Set<Player>()
            .Include(x => x.AppUser)
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendPlayer)
            .FirstOrDefaultAsync(x => x.AppUser!.Id == CurrentUser.Id, ct);
    }
}