using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class AcquireSubLicense(CityVilleDbContext context) : AmfService<AcquireSubLicenseRequest>
{
    public override async Task<ASObject> HandlePacket(AcquireSubLicenseRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var spawner = GameSettingsManager.Instance.GetItem(request.ItemName) ?? throw new Exception($"Game item {request.ItemName} not found");
        var logo = GameSettingsManager.Instance.GetItem(request.SubItemName) ?? throw new Exception($"Game item {request.SubItemName} not found");

        var player = await context.Set<Player>()
            .Include(x => x.SubLicenses.Where(s => s.Name == request.SubItemName))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.AcquireSubLicense(spawner, logo);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class AcquireSubLicenseRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
    [AmfParam(1)] public string SubItemName { get; set; } = string.Empty;
}

public class AcquireSubLicenseRequestValidator : AbstractValidator<AcquireSubLicenseRequest>
{
    public AcquireSubLicenseRequestValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.SubItemName).NotEmpty().MaximumLength(64);
    }
}