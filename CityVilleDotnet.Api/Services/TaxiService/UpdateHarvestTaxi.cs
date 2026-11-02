using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TaxiService;

public class UpdateHarvestTaxi(CityVilleDbContext context) : AmfService<UpdateHarvestTaxiRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateHarvestTaxiRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        if (request.LastResetTime > ServerUtils.GetCurrentTimeSeconds() + 60 || request.LastResetTime - player.TaxiLastResetTime <= GameSettingsManager.Instance.GetSettings().TaxiResetDuration)
        {
            return new CityVilleResponse().Error(GameErrorType.ForceReload).ErrorMessage("Unsynchronized clock with server");
        }

        player.ResetTaxiInstaHarvests(request.LastResetTime);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class UpdateHarvestTaxiRequest
{
    [AmfParam(0)] public double NumInstaHarvests { get; set; }
    [AmfParam(1)] public double LastResetTime { get; set; }
}

public class UpdateHarvestTaxiRequestValidator : AbstractValidator<UpdateHarvestTaxiRequest>
{
    public UpdateHarvestTaxiRequestValidator()
    {
        RuleFor(x => x.NumInstaHarvests).Equal(0);
        RuleFor(x => x.LastResetTime).GreaterThan(0);
    }
}
