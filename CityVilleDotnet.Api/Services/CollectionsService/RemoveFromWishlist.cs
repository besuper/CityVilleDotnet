using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.CollectionsService;

public class RemoveFromWishlist(CityVilleDbContext context) : AmfService<RemoveFromWishlistRequest>
{
    public override async Task<ASObject> HandlePacket(RemoveFromWishlistRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Wishlist)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null)
        {
            throw new Exception("Can't find player");
        }

        player.RemoveFromWishlist(request.ItemName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class RemoveFromWishlistRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}

public class RemoveFromWishlistRequestValidator : AbstractValidator<RemoveFromWishlistRequest>
{
    public RemoveFromWishlistRequestValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}
