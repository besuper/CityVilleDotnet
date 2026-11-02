using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TaxiService;

public class HarvestTaxi(CityVilleDbContext context) : AmfService<HarvestTaxiRequest>
{
    public override async Task<ASObject> HandlePacket(HarvestTaxiRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var taxiDispatches = await context.Set<Player>()
            .Where(x => x.Id == playerId)
            .SelectMany(x => x.Worlds)
            .SelectMany(x => x.Objects)
            .Where(x => x.ClassName == BuildingClassType.TaxiDispatch)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var settings = GameSettingsManager.Instance.GetSettings();

        if (player.Level < 25 || taxiDispatches.Count == 0 || player.TaxiNumInstaHarvests < taxiDispatches.Max(x => x.GetTaxiMaxInstaHarvests()))
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidState);
        }

        if (!settings.AvailableTaxiTypes.Split(',').Contains(request.TaxiName) || request.CoinValue > settings.TaxiHarvestBuildingModifier * settings.TaxiHarvestModifier)
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidData);
        }

        player.AddCoins((int)Math.Round(request.CoinValue));

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class HarvestTaxiRequest
{
    [AmfParam(0)] public string TaxiName { get; set; } = string.Empty;
    [AmfParam(1)] public double CoinValue { get; set; }
}

public class HarvestTaxiRequestValidator : AbstractValidator<HarvestTaxiRequest>
{
    public HarvestTaxiRequestValidator()
    {
        RuleFor(x => x.TaxiName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.CoinValue).GreaterThan(0);
    }
}
