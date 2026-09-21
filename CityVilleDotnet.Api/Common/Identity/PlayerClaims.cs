using System.Security.Claims;

namespace CityVilleDotnet.Api.Common.Identity;

public static class PlayerClaims
{
    public const string PlayerId = "cv:player_id";

    public static Guid? GetPlayerId(this ClaimsPrincipal principal)
    {
        return Guid.TryParse(principal.FindFirstValue(PlayerId), out var playerId) ? playerId : null;
    }
}
