using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class AcquireLicense(CityVilleDbContext context) : AmfService<AcquireLicenseRequest>
{
    public override async Task<ASObject> HandlePacket(AcquireLicenseRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null) throw new Exception($"Game item {request.ItemName} not found");

        if (gameItem.UnlockCost is null)
            throw new Exception($"Game item {request.ItemName} does not have unlock cash defined");

        var player = await context.Set<Player>()
            .Include(x => x.Licenses)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't find player");

        if (player.Cash < gameItem.Cash)
            return new CityVilleResponse().Error(GameErrorType.NotEnoughMoney);

        player.RemoveCash(gameItem.UnlockCost.Value);
        player.AddLicense(request.ItemName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class AcquireLicenseRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}

public class AcquireLicenseValidator : AbstractValidator<AcquireLicenseRequest>
{
    public AcquireLicenseValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}