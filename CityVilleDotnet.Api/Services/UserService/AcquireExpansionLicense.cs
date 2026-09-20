using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class AcquireExpansionLicense(CityVilleDbContext context) : AmfService<AcquireExpansionLicenseRequest>
{
    private const string ExpansionUnlock = "permits_and_population";

    public override async Task<ASObject> HandlePacket(AcquireExpansionLicenseRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.ItemName);

        if (gameItem is null) throw new Exception($"Game item {request.ItemName} not found");

        if (gameItem.Unlock != ExpansionUnlock)
            throw new Exception($"Game item {request.ItemName} is not unlocked with {ExpansionUnlock}");

        var player = await context.Set<Player>()
            .Include(x => x.Licenses)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Can't find player");

        // The client only considers a license valid when its amount is exactly 1
        if (!player.HasLicense(request.ItemName))
            player.AddLicense(request.ItemName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class AcquireExpansionLicenseRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}

public class AcquireExpansionLicenseValidator : AbstractValidator<AcquireExpansionLicenseRequest>
{
    public AcquireExpansionLicenseValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}
