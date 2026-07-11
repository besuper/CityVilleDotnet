using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.ZooService;

public class UnlockNextEnclosure(CityVilleDbContext context) : AmfService<UnlockNextEnclosureRequest>
{
    public override async Task<ASObject> HandlePacket(UnlockNextEnclosureRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.SeenFlags)
            .Include(x => x.InventoryItems)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);

        if (player is null) throw new Exception("Player not found");

        var currentEnclosure = GameSettingsManager.Instance.GetItem(request.ItemName) ?? throw new Exception($"Can't find game item for {request.ItemName}");

        var unlockedItemName = currentEnclosure.UnlocksItem ?? throw new Exception($"{request.ItemName} does not unlock any enclosure");

        player.SetSeenFlag($"zooUnlock_{unlockedItemName}");

        if (!player.HasItem(unlockedItemName))
            player.AddItem(unlockedItemName);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class UnlockNextEnclosureRequest
{
    [AmfParam(0)] public string ItemName { get; set; } = string.Empty;
}

public class UnlockNextEnclosureValidator : AbstractValidator<UnlockNextEnclosureRequest>
{
    public UnlockNextEnclosureValidator()
    {
        RuleFor(x => x.ItemName).NotEmpty().MaximumLength(64);
    }
}
