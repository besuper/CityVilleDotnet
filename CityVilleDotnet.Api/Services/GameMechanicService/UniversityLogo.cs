using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.GameMechanicService;

public class UniversityLogo(CityVilleDbContext context) : AmfService<UniversityLogoRequest>
{
    public override async Task<ASObject> HandlePacket(UniversityLogoRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var logoName = (string)request.ExtraData["propertyName"];

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.ObjectId || o.TempId == request.ObjectId))
            .Include(x => x.SubLicenses.Where(s => s.Name == logoName))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(request.ObjectId);

        var spawner = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        var mechanic = spawner.Mechanics?.GetMechanicByGameMode(request.GameMode)?.GetMechanicItemByType("universityLogo")
                       ?? throw new Exception($"No universityLogo mechanic found for {obj.ItemName} in game mode {request.GameMode}");

        var logo = logoName.Length == 0 ? null : GameSettingsManager.Instance.GetItem(logoName) ?? throw new Exception($"Can't find game item for {logoName}");

        player.SetUniversityLogo(obj, spawner, mechanic, logo);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class UniversityLogoRequest
{
    [AmfParam(0)] public int ObjectId { get; set; }
    [AmfParam(2)] public string GameMode { get; set; } = string.Empty;
    [AmfParam(3)] public Dictionary<string, object> ExtraData { get; set; } = new();
}

public class UniversityLogoRequestValidator : AbstractValidator<UniversityLogoRequest>
{
    public UniversityLogoRequestValidator()
    {
        RuleFor(x => x.GameMode).NotEmpty().MaximumLength(64);
        RuleFor(x => x.ExtraData).Must(x => x.TryGetValue("operation", out var operation) && operation is "setProperty");
        RuleFor(x => x.ExtraData).Must(x => x.TryGetValue("propertyName", out var name) && name is string { Length: <= 64 });
    }
}