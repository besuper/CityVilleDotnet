using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class DeliverCurrency(CityVilleDbContext context) : AmfService<DeliverCurrencyRequest>
{
    public override async Task<ASObject> HandlePacket(DeliverCurrencyRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);
        
        if (player is null) throw new Exception("Player not found");

        switch (request.Currency)
        {
            case "goods":
                player.RemoveGoods(request.Amount);
                break;
            case "coin":
                player.RemoveCoins(request.Amount);
                break;
            default:
                throw new Exception($"Not supported currency {request.Currency}");
        }
        
        player.HandleQuestsProgress("deliver", itemName: request.Currency, amount: request.Amount);

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class DeliverCurrencyRequest
{
    [AmfParam(0)] public string Currency { get; set; } = string.Empty;
    [AmfParam(1)] public int Amount { get; set; }
}