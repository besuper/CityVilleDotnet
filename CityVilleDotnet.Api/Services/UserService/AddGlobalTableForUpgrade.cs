using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class AddGlobalTableForUpgrade(CityVilleDbContext context) : AmfService<AddGlobalTableForUpgradeRequest>
{
    public override async Task<ASObject> HandlePacket(AddGlobalTableForUpgradeRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var objectId = int.Parse(request.ObjectId);

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.TempId == objectId || o.WorldFlatId == objectId))
            .Include(x => x.GlobalTableOverrides)
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var obj = player.GetWorld().GetBuildingByClientId(objectId);
        var gameItem = GameSettingsManager.Instance.GetItem(obj.ItemName) ?? throw new Exception($"Can't find game item for {obj.ItemName}");

        player.AddObjectTableOverrides(obj.WorldFlatId, gameItem.GetGateOverrideTables("pre_upgrade"));

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class AddGlobalTableForUpgradeRequest
{
    [AmfParam(0)] public string ObjectId { get; set; } = string.Empty;
}

public class AddGlobalTableForUpgradeRequestValidator : AbstractValidator<AddGlobalTableForUpgradeRequest>
{
    public AddGlobalTableForUpgradeRequestValidator()
    {
        RuleFor(x => x.ObjectId).NotEmpty().MaximumLength(16).Must(x => int.TryParse(x, out _));
    }
}
