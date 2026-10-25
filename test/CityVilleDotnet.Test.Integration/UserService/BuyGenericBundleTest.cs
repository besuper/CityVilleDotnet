using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class BuyGenericBundleTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task BuyGenericBundle_EnoughCash_AddsRewardsAndDeductsCash()
    {
        var user = Faker.Player();
        user.SetCash(100);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyGenericBundle(Context);
        var request = new BuyGenericBundleRequest { ItemName = "test_generic_bundle" };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>().Include(x => x.InventoryItems).FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);
        player.Cash.Should().Be(80);
        player.CountInventoryItem("test_gate_material_coin").Should().Be(3);
        player.CountInventoryItem("test_res_base").Should().Be(1);
    }

    [Fact]
    public async Task BuyGenericBundle_NotEnoughCash_ThrowsAndGrantsNothing()
    {
        var user = Faker.Player();
        user.SetCash(10);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyGenericBundle(Context);
        var request = new BuyGenericBundleRequest { ItemName = "test_generic_bundle" };

        var act = () => handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<DomainException>();

        var player = await Context.Set<Player>().Include(x => x.InventoryItems).FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);
        player.Cash.Should().Be(10);
        player.CountInventoryItem("test_gate_material_coin").Should().Be(0);
    }

    [Fact]
    public async Task BuyGenericBundle_NotABundle_ThrowsException()
    {
        var user = Faker.Player();

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyGenericBundle(Context);
        var request = new BuyGenericBundleRequest { ItemName = "test_gate_material" };

        var act = () => handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public void BuyGenericBundle_EmptyItemName_FailsValidation()
    {
        var validator = new BuyGenericBundleValidator();
        var request = new BuyGenericBundleRequest { ItemName = "" };

        var result = validator.TestValidate(request);

        result.IsValid.Should().BeFalse();
    }
}
