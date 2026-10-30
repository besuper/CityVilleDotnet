using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.CollectionsService;

public class AddToWishlist(CityVilleDbContext context) : AmfService<AddToWishlistRequest>
{
    public override async Task<ASObject> HandlePacket(AddToWishlistRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        if (GameSettingsManager.Instance.GetItem(request.ItemName) is null)
        {
            throw new Exception($"Can't find item {request.ItemName}");
        }

        var player = await context.Set<Player>()
            .Include(x => x.Wishlist)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null)
        {
            throw new Exception("Can't find player");
        }

        player.AddToWishlist(request.ItemName, GameSettingsManager.Instance.GetSettings().WishlistSize);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class AddToWishlistRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}

public class AddToWishlistRequestValidator : AbstractValidator<AddToWishlistRequest>
{
    public AddToWishlistRequestValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}
