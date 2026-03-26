using Bogus;

namespace CityVilleDotnet.Factory.MapRect;

public static class MapRectFactory
{
    public static Domain.Entities.MapRect MapRect(
        this Faker faker,
        int? x = null,
        int? y = null,
        int? width = null,
        int? height = null)
    {
        return new Domain.Entities.MapRect
        {
            X = x ?? faker.Random.Int(0, 100),
            Y = y ?? faker.Random.Int(0, 100),
            Width = width ?? faker.Random.Int(12, 18),
            Height = height ?? faker.Random.Int(12, 18)
        };
    }
}
