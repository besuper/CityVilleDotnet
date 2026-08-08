using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Data;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.EnumExtensions;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class OnReplaceUserResource(CityVilleDbContext context) : AmfService<OnReplaceUserResourceRequest>
{
    public override async Task<ASObject> HandlePacket(OnReplaceUserResourceRequest request, Guid userId, CancellationToken cancellationToken)
    {
        var gameItem = GameSettingsManager.Instance.GetItem(request.NewResourceName) ?? throw new Exception($"Item {request.NewResourceName} not found");

        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Franchises)
            .ThenInclude(x => x.Locations)
            .Include(x => x.InventoryItems)
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.Objects.Where(o => o.WorldFlatId == request.OldResourceId || o.TempId == request.OldResourceId))
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken) ?? throw new Exception("Player not found");

        if (request.PlayerUid != player.Snuid)
            throw new Exception("Player uid mismatch");

        var isCitySamMap = request.WorldOwnerId == SamanthaSeeder.SamanthaSnuid;
        
        if (!isCitySamMap && request.WorldOwnerId != player.Snuid)
            throw new Exception("World owner mismatch");

        if (!request.IsGift)
        {
            if (gameItem.Cost > 0)
                player.RemoveCoins(gameItem.Cost.Value);
            else if (gameItem.Cash > 0)
                player.RemoveCash(gameItem.Cash.Value);

            if (gameItem.XpYield > 0)
                player.AddXp(gameItem.XpYield.Value);
        }

        if (isCitySamMap)
        {
            var franchise = player.Franchises.FirstOrDefault(x => x.FranchiseType == request.NewResourceName)
                            ?? throw new Exception($"Can't find franchise {request.NewResourceName}");

            franchise.AddCitySamLocation(request.OldResourceId, gameItem.GetCommodityRequired() ?? 1);
            
            player.HandleQuestsProgress("citySamHQ");
            player.CheckCompletedQuests();
        }
        else
        {
            var obj = player.GetWorld().GetBuildingByClientId(request.OldResourceId) ?? throw new Exception($"Building {request.OldResourceId} not found");

            var className = Enum.Parse<BuildingClassType>(gameItem.Type.Pascalize());

            obj.ReplaceWith(request.NewResourceName, className, obj.Direction, obj.X, obj.Y, obj.Z, obj.State);

            if (request.IsUsingConstruction && gameItem.Construction is not null)
            {
                var constructionItem = GameSettingsManager.Instance.GetItem(gameItem.Construction);

                if (constructionItem?.NumberOfStages is null)
                    throw new Exception($"Construction item not found with {gameItem.Construction}");

                obj.SetAsConstructionSite(gameItem.Construction, constructionItem.NumberOfStages.Value);
            }
            else if (className.IsBusiness())
            {
                obj.Close();
            }
        }

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class OnReplaceUserResourceRequest
{
    [AmfParam(0)] public int WorldOwnerId { get; set; }
    [AmfParam(1)] public int PlayerUid { get; set; }
    [AmfParam(2)] public int OldResourceId { get; set; }
    [AmfParam(3)] public string NewResourceName { get; set; } = string.Empty;
    [AmfParam(4)] public bool IsGift { get; set; }
    [AmfParam(5)] public bool IsUsingConstruction { get; set; }
}

public class OnReplaceUserResourceValidator : AbstractValidator<OnReplaceUserResourceRequest>
{
    public OnReplaceUserResourceValidator()
    {
        RuleFor(x => x.NewResourceName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.OldResourceId).GreaterThan(0);
    }
}
