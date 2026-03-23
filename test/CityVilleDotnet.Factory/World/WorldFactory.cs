using Bogus;
using CityVilleDotnet.Domain.Entities;

namespace CityVilleDotnet.Factory.World;

public static class WorldFactory
{
    public static Domain.Entities.World World(
        this Faker faker,
        string? worldName = null,
        int? sizeX = null,
        int? sizeY = null,
        int? population = null,
        int? populationMin = null,
        int? populationMax = null,
        int? populationCap = null,
        int? potentialPopulation = null,
        List<MapRect>? mapRects = null,
        List<Domain.Entities.WorldObject>? objects = null)
    {
        return new Domain.Entities.World(
            worldName ?? faker.Random.String2(32),
            sizeX: sizeX ?? 36,
            sizeY: sizeY ?? 36,
            population: population ?? 0,
            populationMin: populationMin ?? 0,
            populationMax: populationMax ?? 50,
            populationCap: populationCap ?? 0,
            potentialPopulation: potentialPopulation ?? 0,
            mapRects: mapRects ?? [],
            objects: objects ?? []);
    }
}
