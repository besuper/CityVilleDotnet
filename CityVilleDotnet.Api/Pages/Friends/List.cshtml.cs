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
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly CityVilleDbContext _dbContext = dbContext;

    public ApplicationUser CurrentUser { get; set; }
    public List<FriendDto> Friends { get; set; }

    [BindProperty]
    public string Username { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        CurrentUser = await _userManager.GetUserAsync(User);

        if (CurrentUser is null)
            return RedirectToPage("/Account/Login");

        Friends = await _dbContext.Set<User>()
            .AsNoTracking()
            .Where(x => x.AppUser!.Id.Equals(CurrentUser.Id))
            .Include(x => x.AppUser)
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .SelectMany(x => x.Friends, (_, friend) => friend)
            .Select(x => x.ToDto())
            .ToListAsync(ct);

        return Page();
    }

    public async Task<IActionResult> OnPostAddFriend(CancellationToken ct)
    {
        CurrentUser = await _userManager.GetUserAsync(User);

        if (CurrentUser is null)
            return RedirectToPage("/Account/Login");

        var user = await _dbContext.Set<User>()
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .Include(x => x.AppUser)
            .FirstOrDefaultAsync(x => x.AppUser!.Id.Equals(CurrentUser.Id), ct);

        if (user is null)
            return RedirectToPage("/Account/Login");

        var targetUser = await _dbContext.Set<User>()
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .Where(x => x.UserInfo!.Username.Equals(Username))
            .FirstOrDefaultAsync(ct);

        if (targetUser is null) return RedirectToPage("/Friends/List");

        var alreadyFriend = user.Friends.Any(x => x.FriendUser.UserInfo.Username.Equals(Username));

        if (alreadyFriend)
            // TODO: Show error
            return RedirectToPage("/Friends/List");

        var friendship1 = new Friend(targetUser, user, true);
        var friendship2 = new Friend(user, targetUser, false);

        targetUser.Friends.Add(friendship1);
        user.Friends.Add(friendship2);

        await _dbContext.SaveChangesAsync(ct);

        return RedirectToPage("/Friends/List");
    }

    public async Task<IActionResult> OnGetAccept(string userName, CancellationToken ct)
    {
        CurrentUser = await _userManager.GetUserAsync(User);

        if (CurrentUser is null)
            return RedirectToPage("/Account/Login");

        var user = await _dbContext.Set<User>()
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .Include(x => x.UserInfo)
            .ThenInclude(x => x.Player)
            .Include(x => x.AppUser)
            .Include(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.Friends)
            .ThenInclude(x => x.FriendUser)
            .ThenInclude(x => x.UserInfo)
            .FirstOrDefaultAsync(x => x.AppUser!.Id.Equals(CurrentUser.Id), ct);

        if (user is null)
            return RedirectToPage("/Account/Login");

        var friendship = user.Friends.FirstOrDefault(x => x.FriendUser.UserInfo.Username.Equals(userName));
        friendship.Status = FriendshipStatus.Accepted;

        var targetFriendship = friendship.FriendUser.Friends.FirstOrDefault(x => x.FriendUser.UserInfo.Username.Equals(user.UserInfo.Username));
        targetFriendship.Status = FriendshipStatus.Accepted;

        await _dbContext.SaveChangesAsync(ct);

        return RedirectToPage("/Friends/List");
    }
}
