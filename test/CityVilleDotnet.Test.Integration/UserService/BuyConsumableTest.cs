using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class BuyConsumableTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task BuyConsumable_CashItem_AddsItemAndDeductsCash()
    {
        var user = Faker.Player();
        user.SetCash(100);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new BuyConsumable(Context, NullLogger<BuyConsumable>.Instance);
        var request = new BuyConsumableRequest { ItemName = "test_gate_material", Amount = 2 };

        var response = await handler.HandlePacket(request, user.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>().Include(x => x.InventoryItems).FirstAsync(x => x.Id == user.Id);
        player.Cash.Should().Be(94);
        player.CountInventoryItem("test_gate_material").Should().Be(2);
    }

    [Fact]
    public async Task BuyConsumable_CoinItem_AddsItemAndDeductsCoins()
    {
        var user = Faker.Player();
        user.SetGold(1000);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new BuyConsumable(Context, NullLogger<BuyConsumable>.Instance);
        var request = new BuyConsumableRequest { ItemName = "test_gate_material_coin", Amount = 1 };

        var response = await handler.HandlePacket(request, user.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>().Include(x => x.InventoryItems).FirstAsync(x => x.Id == user.Id);
        player.Gold.Should().Be(950);
        player.CountInventoryItem("test_gate_material_coin").Should().Be(1);
    }

    [Theory]
    [InlineData("", 1)]
    [InlineData("test_gate_material", 0)]
    public void BuyConsumable_InvalidRequest_FailsValidation(string itemName, int amount)
    {
        var validator = new BuyConsumableValidator();
        var request = new BuyConsumableRequest { ItemName = itemName, Amount = amount };

        var result = validator.TestValidate(request);

        result.IsValid.Should().BeFalse();
    }
}
