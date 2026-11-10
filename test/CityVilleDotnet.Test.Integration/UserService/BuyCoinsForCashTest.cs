using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class BuyCoinsForCashTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private async Task<Player> SeedPlayer(int gold, int cash)
    {
        var player = Faker.Player();
        player.SetGold(gold);
        player.SetCash(cash);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return player;
    }

    [Fact]
    public async Task BuyCoinsForCash_EnoughCash_SavesConvertedCoins()
    {
        var player = await SeedPlayer(gold: 500, cash: 10);

        var response = await new BuyCoinsForCash(Context).HandlePacket(new BuyCoinsForCashRequest { Cash = 3 }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);
        var saved = await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        saved.Cash.Should().Be(7);
        saved.Gold.Should().Be(500 + 3 * GameSettingsManager.Instance.GetSettings().CoinsForCash);
    }

    [Fact]
    public async Task BuyCoinsForCash_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var player = await SeedPlayer(gold: 500, cash: 2);

        var act = () => new BuyCoinsForCash(Context).HandlePacket(new BuyCoinsForCashRequest { Cash = 3 }, player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        var saved = await Context.Set<Player>().AsNoTracking().FirstAsync(x => x.Id == player.Id, TestContext.Current.CancellationToken);
        saved.Cash.Should().Be(2);
        saved.Gold.Should().Be(500);
    }
}
