using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class BuyPermitPackTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private async Task<Player> SeedPlayer(int cash)
    {
        var player = Faker.Player();
        player.SetCash(cash);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return player;
    }

    [Fact]
    public async Task BuyPermitPack_EnoughCash_AddsPermitsAndRemovesCash()
    {
        var player = await SeedPlayer(cash: 20);

        var response = await new BuyPermitPack(Context).HandlePacket(new BuyPermitPackRequest { ItemName = "test_permit_pack" }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var saved = await Context.Set<Player>().AsNoTracking().Include(x => x.InventoryItems).FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        saved.Cash.Should().Be(5);
        saved.CountInventoryItem("test_permits").Should().Be(5);
    }

    [Fact]
    public async Task BuyPermitPack_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var player = await SeedPlayer(cash: 10);

        var act = () => new BuyPermitPack(Context).HandlePacket(new BuyPermitPackRequest { ItemName = "test_permit_pack" }, player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        player.Cash.Should().Be(10);
        player.CountInventoryItem("test_permits").Should().Be(0);
    }

    [Fact]
    public async Task BuyPermitPack_NotAPermitPack_Throws()
    {
        var player = await SeedPlayer(cash: 20);

        var act = () => new BuyPermitPack(Context).HandlePacket(new BuyPermitPackRequest { ItemName = "test_generic_bundle" }, player.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        player.Cash.Should().Be(20);
    }
}
