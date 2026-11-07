using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public class FinalizeFranchisePendingName(CityVilleDbContext context) : AmfService<FinalizeFranchisePendingNameRequest>
{
    public override async Task<ASObject> HandlePacket(FinalizeFranchisePendingNameRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Franchises.Where(f => f.FranchiseType == request.FranchiseType))
            .ThenInclude(x => x.Locations)
            .Include(x => x.RequestInventoryItems.Where(i => i.InventoryName == request.FranchiseType))
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.ClassName == BuildingClassType.Municipal))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var name = player.FinalizeFranchisePendingName(request.FranchiseType);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["name"] = name
        });
    }
}

public class FinalizeFranchisePendingNameRequest
{
    [AmfParam(0)] public string FranchiseType { get; set; } = string.Empty;
}

public class FinalizeFranchisePendingNameRequestValidator : AbstractValidator<FinalizeFranchisePendingNameRequest>
{
    public FinalizeFranchisePendingNameRequestValidator()
    {
        RuleFor(x => x.FranchiseType).NotEmpty().MaximumLength(64);
    }
}
