using Bogus;
using CityVilleDotnet.Domain.Enums;

namespace CityVilleDotnet.Factory.WorldObject;

public static class WorldObjectFactory
{
    public static Domain.Entities.WorldObject WorldObject(
        this Faker faker,
        string? itemName = null,
        BuildingClassType? className = null,
        string? contractName = null,
        bool deleted = false,
        int tempId = -1,
        WorldObjectState state = WorldObjectState.Static,
        int direction = 0,
        double? buildTime = null,
        double? plantTime = null,
        int? x = null,
        int? y = null,
        int z = 0,
        int? worldFlatId = null)
    {
        return new Domain.Entities.WorldObject(
            itemName ?? faker.Random.String2(64),
            className ?? BuildingClassType.Residence,
            contractName,
            deleted,
            tempId,
            state,
            direction,
            buildTime,
            plantTime,
            x ?? faker.Random.Int(0, 35),
            y ?? faker.Random.Int(0, 35),
            z,
            worldFlatId ?? faker.Random.Int(1, 9999));
    }
}
