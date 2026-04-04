using CityVilleDotnet.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Pages.Account;

public class RegisterModel(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    CityVilleDbContext context) : PageModel
{
    [BindProperty] public required RegisterInputModel Input { get; set; }

    public required string ReturnUrl { get; set; } = "/Game";

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl = returnUrl ?? "/Game";

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new ApplicationUser
        {
            UserName = Input.Username,
        };

        var result = await userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            var jsonContent = await System.IO.File.ReadAllTextAsync("Resources/startWorld.json");
            var defaultWorld = JsonSerializer.Deserialize<WorldDto>(jsonContent) ?? throw new Exception("WorldDto can't be null");

            var newUser = CityVilleDotnet.Domain.Entities.User.CreateNewPlayer(defaultWorld, user);

            await context.AddAsync(newUser);

            var samantha = await context.Set<Player>()
                .Include(u => u.Friends)
                .FirstOrDefaultAsync(u => u.Snuid == -1);

            if (samantha is not null)
            {
                var friendship1 = new Friend(samantha, newUser.GetPlayer(), true) { Status = FriendshipStatus.Accepted };
                var friendship2 = new Friend(newUser.GetPlayer(), samantha, false) { Status = FriendshipStatus.Accepted };

                samantha.Friends.Add(friendship1);
                newUser.GetPlayer().Friends.Add(friendship2);
            }

            await context.SaveChangesAsync();

            await signInManager.SignInAsync(user, isPersistent: false);
            return Redirect(returnUrl);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }
}