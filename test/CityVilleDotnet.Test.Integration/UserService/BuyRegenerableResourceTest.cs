using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

// See test_gas_cash in TestData/gameSettings.xml
[Collection("Database")]
public class BuyRegenerableResourceTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task BuyRegenerableResource_CashItem_DeductsCashAndAddsGas()
    {
        var user = Faker.Player();
        user.SetCash(100);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await new BuyRegenerableResource(Context).HandlePacket(new BuyRegenerableResourceRequest { ItemName = "test_gas_cash" }, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>().FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);
        player.Cash.Should().Be(95);
        player.Gas.Should().Be(17);
    }

    [Fact]
    public async Task BuyRegenerableResource_ItemWithoutGas_ThrowsInvalidData()
    {
        var user = Faker.Player();

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var act = () => new BuyRegenerableResource(Context).HandlePacket(new BuyRegenerableResourceRequest { ItemName = "deco_tree" }, user.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidData);
    }
}
