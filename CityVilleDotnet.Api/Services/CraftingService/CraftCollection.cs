using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.CraftingService;

public class CraftCollection(CityVilleDbContext context) : AmfService<CraftCollectionRequest>
{
    public override async Task<ASObject> HandlePacket(CraftCollectionRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var recipe = GameSettingsManager.Instance.GetCraftCollection(request.CollectionName);

        if (recipe is null)
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidData);
        }

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.CraftingParts)
            .Include(x => x.Cars)
            .Include(x => x.InventoryItems)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        if (!player.CraftCollection(recipe))
        {
            return new CityVilleResponse().Error(GameErrorType.InvalidState);
        }

        player.HandleQuestsProgress("craftCollection", itemName: recipe.Name);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse();
    }
}

public class CraftCollectionRequest
{
    [AmfParam(0)] public string CollectionName { get; set; } = string.Empty;
}

public class CraftCollectionRequestValidator : AbstractValidator<CraftCollectionRequest>
{
    public CraftCollectionRequestValidator()
    {
        RuleFor(x => x.CollectionName).NotEmpty().MaximumLength(64);
    }
}
