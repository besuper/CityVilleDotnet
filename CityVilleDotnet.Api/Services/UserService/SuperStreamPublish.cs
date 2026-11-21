using System.Text.Json;
using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Domain.GameEntities;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class SuperStreamPublish(CityVilleDbContext context) : AmfService<SuperStreamPublishRequest>
{
    public override async Task<ASObject> HandlePacket(SuperStreamPublishRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        Player? targetPlayer = null;

        if (int.TryParse(request.TargetId, out var targetSnuid) && targetSnuid != player.Snuid)
        {
            targetPlayer = await context.Set<Player>().FirstOrDefaultAsync(x => x.Snuid == targetSnuid, cancellationToken);
        }

        player.Publish(SocialPostType.Feed, request.Type, JsonSerializer.Serialize(request.Data), request.UserMessage, targetPlayer,
            request.ClickthroughParams?.TargetObjectId ?? 0, request.ClickthroughParams?.TargetObjectName, request.FeedData?.Material,
            ServerUtils.TimeProvider.GetUtcNow().UtcDateTime);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(AmfConverter.Convert(new FeedPostResultDto())!);
    }
}

public class SuperStreamPublishRequest
{
    [AmfParam(0)] public string Type { get; set; } = string.Empty;
    [AmfParam(1)] public ASObject Data { get; set; } = [];
    [AmfParam(1)] public SuperStreamPublishFeedData? FeedData { get; set; }
    [AmfParam(2)] public string? TargetId { get; set; }
    [AmfParam(3)] public SuperStreamPublishClickthroughParams? ClickthroughParams { get; set; }
    [AmfParam(5)] public string? UserMessage { get; set; }
}

public class SuperStreamPublishFeedData
{
    [AmfParam("material")] public string? Material { get; set; }
}

public class SuperStreamPublishClickthroughParams
{
    [AmfParam("targetObjectId")] public int TargetObjectId { get; set; }
    [AmfParam("targetObjectName")] public string? TargetObjectName { get; set; }
}

public class SuperStreamPublishRequestValidator : AbstractValidator<SuperStreamPublishRequest>
{
    public SuperStreamPublishRequestValidator()
    {
        RuleFor(x => x.Type).NotEmpty().MaximumLength(128);
        RuleFor(x => x.FeedData!.Material).MaximumLength(128).When(x => x.FeedData is not null);
        RuleFor(x => x.ClickthroughParams!.TargetObjectName).MaximumLength(128).When(x => x.ClickthroughParams is not null);
    }
}