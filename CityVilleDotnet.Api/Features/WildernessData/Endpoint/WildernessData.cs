using FastEndpoints;

namespace CityVilleDotnet.Api.Features.WildernessData.Endpoint;

internal sealed class WildernessData : Endpoint<WildernessDataRequest>
{
    public override void Configure()
    {
        Get("/assets/wilderness/WildernessData/WildernessClientData{Uid}.txt");
    }

    public override async Task HandleAsync(WildernessDataRequest req, CancellationToken ct)
    {
        await Send.RedirectAsync("/assets/wilderness/WildernessData/WildernessClientData14.txt");
    }
}

internal sealed record WildernessDataRequest(
    string Uid
);