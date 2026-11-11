using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.CarService;

public class CarCraftBuyAll(CityVilleDbContext context) : AmfService<CarCraftBuyAllRequest>
{
    public override async Task<ASObject> HandlePacket(CarCraftBuyAllRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var recipe = GameSettingsManager.Instance.GetCraftCollection(request.RecipeName);

        if (recipe is null)
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidData);
        }

        var partNames = recipe.Collectables.Select(x => x.Name).ToList();

        var player = await context.Set<Player>()
            .Include(x => x.CraftingParts.Where(p => partNames.Contains(p.Name)))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        if (!player.CarCraftEnabled)
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidState);
        }

        player.BuyMissingCraftParts(recipe, GameSettingsManager.Instance.GetSettings().BulkCarCraftPurchaseDiscount);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class CarCraftBuyAllRequest
{
    [AmfParam(0)] public string RecipeName { get; set; } = string.Empty;
}

public class CarCraftBuyAllRequestValidator : AbstractValidator<CarCraftBuyAllRequest>
{
    public CarCraftBuyAllRequestValidator()
    {
        RuleFor(x => x.RecipeName).NotEmpty().MaximumLength(64);
    }
}
