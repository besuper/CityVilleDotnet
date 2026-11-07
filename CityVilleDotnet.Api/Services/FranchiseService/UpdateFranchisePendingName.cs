using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.FranchiseService;

public class UpdateFranchisePendingName(CityVilleDbContext context) : AmfService<UpdateFranchisePendingNameRequest>
{
    public override async Task<ASObject> HandlePacket(UpdateFranchisePendingNameRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Franchises.Where(f => f.FranchiseType == request.FranchiseType))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        player.UpdateFranchisePendingName(request.FranchiseType, request.FranchiseName);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject
        {
            ["name"] = request.FranchiseName.Trim()
        });
    }
}

public class UpdateFranchisePendingNameRequest
{
    [AmfParam(0)] public string FranchiseType { get; set; } = string.Empty;
    [AmfParam(1)] public string FranchiseName { get; set; } = string.Empty;
}

public class UpdateFranchisePendingNameRequestValidator : AbstractValidator<UpdateFranchisePendingNameRequest>
{
    public UpdateFranchisePendingNameRequestValidator()
    {
        RuleFor(x => x.FranchiseType).NotEmpty().MaximumLength(64);
        RuleFor(x => x.FranchiseName).NotEmpty().MaximumLength(30);
    }
}
