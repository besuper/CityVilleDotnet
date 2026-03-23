using AwesomeAssertions;
using CityVilleDotnet.Api.Services.WorldService;
using CityVilleDotnet.Api.Services.WorldService.Common;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.User;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.WorldService;

[Collection("Database")]
public class MoveTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task Move_ValidRequest_UpdatesBuildingPosition()
    {
        var building = Faker.WorldObject(x: 10, y: 20, direction: 0);
        var world = Faker.World(objects: [building]);
        var user = Faker.User(world: world);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new Move(Context);
        var request = new MoveRequest
        {
            Building = new MoveBuildingRequest
            {
                Position = new PerformActionPositionRequest { X = 30, Y = 40, Z = 0 },
                Direction = 2
            },
            MoveParams = [new MoveParamsRequest { OrigX = 10, OrigY = 20 }]
        };

        var response = await handler.HandlePacket(request, user.UserId, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var updatedObj = await Context.Set<WorldObject>().FirstAsync(x => x.Id == building.Id);
        updatedObj.X.Should().Be(30);
        updatedObj.Y.Should().Be(40);
        updatedObj.Direction.Should().Be(2);
    }

    [Fact]
    public async Task Move_BuildingNotFound_ThrowsException()
    {
        var building = Faker.WorldObject(x: 10, y: 20, direction: 0);
        var world = Faker.World(objects: [building]);
        var user = Faker.User(world: world);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new Move(Context);
        var request = new MoveRequest
        {
            Building = new MoveBuildingRequest
            {
                Position = new PerformActionPositionRequest { X = 30, Y = 40, Z = 0 },
                Direction = 0
            },
            MoveParams = [new MoveParamsRequest { OrigX = 15, OrigY = 35 }]
        };

        var act = () => handler.HandlePacket(request, user.UserId, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }
}