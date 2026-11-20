using Bogus;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;

namespace CityVilleDotnet.Factory.SocialPost;

public static class SocialPostFactory
{
    public static Domain.Entities.SocialPost SocialPost(
        this Faker faker,
        Domain.Entities.Player? author = null,
        string? key = null,
        string? userMessage = null,
        Domain.Entities.Player? targetPlayer = null,
        DateTime? now = null)
    {
        return new Domain.Entities.SocialPost(
            author ?? faker.Player(),
            SocialPostType.Feed,
            key ?? faker.Random.String2(128),
            "{}",
            userMessage,
            targetPlayer,
            now ?? faker.Date.Recent().ToUniversalTime());
    }
}
