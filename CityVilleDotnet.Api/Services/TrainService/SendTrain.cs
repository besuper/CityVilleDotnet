using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.TrainService;

public class SendTrain(CityVilleDbContext context) : AmfService<SendTrainRequest>
{
    public override async Task<ASObject> HandlePacket(SendTrainRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .AsSplitQuery()
            .Include(x => x.Worlds.Where(w => w.Type == w.Player!.LastPlayedWorldType))
            .ThenInclude(x => x.TrainOrder)
            .Include(x => x.Quests.Where(q => q.QuestType == QuestType.Active))
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        var attributes = request.Attributes;
        var item = GameSettingsManager.Instance.GetItem(attributes.TrainName) ?? throw new Exception($"Can't find train schedule {attributes.TrainName}");

        if (item.TrainPayout is null)
            throw new Exception($"{item.Name} is not a train schedule");

        if (player.Level < item.RequiredLevel)
            throw new Exception($"Level {item.RequiredLevel} is required to send {item.Name}");

        if (item.Goods > 0)
        {
            if (attributes.Operation != TrainOperationType.Sell)
                throw new Exception($"{item.Name} can only be sent as a sell order");

            player.RemoveGoods(item.Goods.Value);
        }
        else
        {
            if (attributes.Operation != TrainOperationType.Buy)
                throw new Exception($"{item.Name} can only be sent as a buy order");

            player.RemoveCoins(item.Cost ?? 0);
        }

        player.GetWorld().StartTrainOrder(item.Name, attributes.Operation, attributes.CommodityName, ServerUtils.GetCurrentTimeSeconds());
        player.HandleQuestsProgress("sendTrain");

        await context.SaveChangesAsync(cancellationToken);

        return GatewayService.CreateEmptyResponse();
    }
}

public class SendTrainRequest
{
    [AmfParam(0)] public SendTrainAttributesRequest Attributes { get; set; } = new();
}

public class SendTrainAttributesRequest
{
    [AmfParam("trainName")] public string TrainName { get; set; } = string.Empty;
    [AmfParam("operation")] public TrainOperationType Operation { get; set; }
    [AmfParam("commodityName")] public string CommodityName { get; set; } = string.Empty;
}

public class SendTrainValidator : AbstractValidator<SendTrainRequest>
{
    public SendTrainValidator()
    {
        RuleFor(x => x.Attributes.TrainName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Attributes.CommodityName).NotEmpty().MaximumLength(32);
    }
}
