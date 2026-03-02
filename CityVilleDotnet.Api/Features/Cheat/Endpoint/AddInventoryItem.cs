using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Features.Cheat.Endpoint;

internal sealed class AddInventoryItem(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext, IConfiguration configuration) : Endpoint<AddInventoryItemRequest>
{
    public override void Configure()
    {
        Post("/api/cheat/inventory");
    }

    public override async Task HandleAsync(AddInventoryItemRequest req, CancellationToken ct)
    {
        var enableCheat = configuration.GetValue<bool>("enableCheat");

        if (!enableCheat)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var currentUser = await userManager.GetUserAsync(HttpContext.User);

        if (currentUser is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var player = await dbContext.Set<User>()
            .Where(x => x.AppUser!.Id == currentUser.Id)
            .Include(x => x.Player)
            .ThenInclude(x => x!.InventoryItems)
            .Select(x => x.Player)
            .FirstOrDefaultAsync(ct);

        if (player is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        player.AddItem(req.ItemName, req.Amount);

        await dbContext.SaveChangesAsync(ct);

        await Send.OkAsync(cancellation: ct);
    }
}

internal sealed class AddInventoryItemRequest
{
    public required string ItemName { get; set; }
    public int Amount { get; set; } = 1;
}