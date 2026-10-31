using AwesomeAssertions;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Api.Services.GameMechanicService;
using CityVilleDotnet.Test.Integration.Fixtures;

namespace CityVilleDotnet.Test.Integration.GameMechanicService;

// Every GlobalTableModifierMechanic sends its own arbitrary type (e.g. picklejarstable), they must all reach GlobalTableModifiers
[Collection("Database")]
public class GlobalTableModifiersTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Theory]
    [InlineData("globalTableModifiers")]
    [InlineData("test_custom_table")]
    [InlineData("test_toggled_load_table")]
    [InlineData("test_postexplode_table")]
    public void ResolveMechanicHandler_GlobalTableModifierType_RoutesToGlobalTableModifiers(string mechanicType)
    {
        GatewayService.ResolveMechanicHandler(mechanicType).Should().Be(nameof(GlobalTableModifiers));
    }

    [Theory]
    [InlineData("tutorial", nameof(Tutorial))]
    [InlineData("harvestState", nameof(HarvestState))]
    public void ResolveMechanicHandler_OtherMechanicType_RoutesToPascalizedType(string mechanicType, string expected)
    {
        GatewayService.ResolveMechanicHandler(mechanicType).Should().Be(expected);
    }

    [Fact]
    public async Task GlobalTableModifiers_ReturnsSuccess()
    {
        var handler = new GlobalTableModifiers();

        var response = await handler.HandlePacket([1, "test_custom_table", "onAddToWorld", new object()], Guid.NewGuid(), TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
    }
}
