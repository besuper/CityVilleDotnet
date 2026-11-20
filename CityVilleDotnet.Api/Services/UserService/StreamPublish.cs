using System.Text.Json;
using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FluentValidation;
using FluorineFx;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Services.UserService;

public class StreamPublish(CityVilleDbContext context) : AmfService<StreamPublishRequest>
{
    public override async Task<ASObject> HandlePacket(StreamPublishRequest request, Guid playerId, CancellationToken cancellationToken)
    {
        var player = await context.Set<Player>()
            .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken) ?? throw new Exception("Player not found");

        Player? targetPlayer = null;

        if (int.TryParse(request.TargetId, out var targetSnuid) && targetSnuid != player.Snuid)
        {
            targetPlayer = await context.Set<Player>()
                .FirstOrDefaultAsync(x => x.Snuid == targetSnuid, cancellationToken);
        }

        player.Publish(SocialPostType.Feed, request.Type, JsonSerializer.Serialize(request.Data), request.UserMessage, targetPlayer, ServerUtils.TimeProvider.GetUtcNow().UtcDateTime);

        await context.SaveChangesAsync(cancellationToken);

        return new CityVilleResponse().Data(new ASObject { ["lastSuperFeedPostError"] = 0 });
    }
}

public class StreamPublishRequest
{
    [AmfParam(0)] public string Type { get; set; } = string.Empty;
    [AmfParam(1)] public ASObject Data { get; set; } = [];
    [AmfParam(2)] public string? TargetId { get; set; }
    [AmfParam(6)] public string? UserMessage { get; set; }
}

public class StreamPublishRequestValidator : AbstractValidator<StreamPublishRequest>
{
    public StreamPublishRequestValidator()
    {
        RuleFor(x => x.Type).NotEmpty().MaximumLength(128);
    }
}
