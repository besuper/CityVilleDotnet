using System.Text.Json;
using CityVilleDotnet.Api.Common.Identity;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Api.Features.Social.Endpoint;

internal sealed class SendRequest(CityVilleDbContext dbContext) : Endpoint<SendRequestRequest, SendRequestResponse>
{
    public override void Configure()
    {
        Post("/social/send-request");
    }

    public override async Task HandleAsync(SendRequestRequest req, CancellationToken ct)
    {
        if (HttpContext.User.GetPlayerId() is not { } playerId)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var (type, clientType) = req.Handler == "sendGiftRequest" ? (SocialPostType.Gift, "gift") : (SocialPostType.Request, "requestItem");

        var player = await dbContext.Set<Player>()
            .Include(x => x.Friends.Where(f => f.Status == FriendshipStatus.Accepted))
            .ThenInclude(x => x.FriendPlayer)
            .FirstOrDefaultAsync(x => x.Id == playerId, ct);

        if (player is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var snuids = req.Ids
            .Select(x => int.TryParse(x, out var snuid) ? snuid : (int?)null)
            .OfType<int>()
            .ToList();

        var recipients = player.SendRequest(type, req.ItemName, req.Data.GetRawText(), snuids, ServerUtils.TimeProvider.GetUtcNow().UtcDateTime);

        if (recipients.Count == 0)
        {
            await Send.OkAsync(new SendRequestResponse { Error = "no_recipients", Type = clientType }, ct);
            return;
        }

        await dbContext.SaveChangesAsync(ct);

        await Send.OkAsync(new SendRequestResponse { Error = "none", Type = clientType, Ids = recipients.Select(x => x.Snuid.ToString()).ToList() }, ct);
    }
}

internal sealed class SendRequestRequest
{
    public string Handler { get; set; } = string.Empty;
    public List<string> Ids { get; set; } = [];
    public JsonElement Data { get; set; }

    public string ItemName => Data.GetProperty("item").GetString()!;
}

internal sealed class SendRequestValidator : Validator<SendRequestRequest>
{
    public SendRequestValidator()
    {
        RuleFor(x => x.Handler).Must(x => x is "sendGiftRequest" or "sendItemRequest");
        RuleFor(x => x.Ids).Must(x => x is not null && x.Count <= 300);
        RuleFor(x => x.Data).Must(BeValidData);
    }

    private static bool BeValidData(JsonElement data)
    {
        return data.ValueKind == JsonValueKind.Object
               && data.GetRawText().Length <= 4096
               && data.TryGetProperty("item", out var item)
               && item.ValueKind == JsonValueKind.String
               && GameSettingsManager.Instance.GetItem(item.GetString()!) is not null;
    }
}

internal sealed class SendRequestResponse
{
    public required string Error { get; set; }
    public required string Type { get; set; }
    public List<string> Ids { get; set; } = [];
}
