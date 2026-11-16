using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class UpdateRegenerableResourceTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task UpdateRegenerableResource_NewPlayer_StoresRegeneratedGas()
    {
        var user = Faker.Player();

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await new UpdateRegenerableResource(Context).HandlePacket(new UpdateRegenerableResourceRequest { ResourceName = "gas" }, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>().FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);
        player.Gas.Should().Be(12);
    }

    [Fact]
    public void UpdateRegenerableResourceRequestValidator_UnknownResource_HasError()
    {
        var result = new UpdateRegenerableResourceRequestValidator().TestValidate(new UpdateRegenerableResourceRequest { ResourceName = "water" });

        result.ShouldHaveValidationErrorFor(x => x.ResourceName);
    }
}
