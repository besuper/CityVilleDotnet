using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Features.Cheat.Endpoint;

internal sealed class UpdateResources(UserManager<ApplicationUser> userManager, CityVilleDbContext dbContext, IConfiguration configuration) : Endpoint<UpdateResourcesRequest>
{
    public override void Configure()
    {
        Post("/api/cheat/resources");
    }

    public override async Task HandleAsync(UpdateResourcesRequest req, CancellationToken ct)
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

        var player = await dbContext.Set<Player>()
            .Include(x => x.AppUser)
            .FirstOrDefaultAsync(x => x.AppUser!.Id == currentUser.Id, ct);

        if (player is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        if (req.Gold.HasValue) player.SetGold(req.Gold.Value);
        if (req.Cash.HasValue) player.SetCash(req.Cash.Value);
        if (req.Goods.HasValue) player.SetGoods(req.Goods.Value);
        if (req.PremiumGoods.HasValue) player.SetPremiumGoods(req.PremiumGoods.Value);
        if (req.Level.HasValue) player.SetLevel(req.Level.Value);
        if (req.Xp.HasValue) player.SetXp(req.Xp.Value);

        await dbContext.SaveChangesAsync(ct);

        await Send.OkAsync(cancellation: ct);
    }
}

internal sealed class UpdateResourcesRequest
{
    public int? Gold { get; set; }
    public int? Cash { get; set; }
    public int? Xp { get; set; }
    public int? Level { get; set; }
    public int? Goods { get; set; }
    public int? PremiumGoods { get; set; }
}