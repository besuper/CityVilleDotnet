using AwesomeAssertions;
using CityVilleDotnet.Api.Services.MasteryGateService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.MasteryGateService;

[Collection("Database")]
public class BuyMasteryGateTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task BuyMasteryGate_CropGate_ChargesProratedCostAndSetsFlag()
    {
        var user = Faker.Player();
        user.SetCash(150);
        user.IncrementMastery("test_mastery_crop_a", 30);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyMasteryGate(Context);
        var request = new BuyMasteryGateRequest { ItemName = "test_mastery_gate", GateName = "mastery_crop" };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.Cash.Should().Be(125);
        player.SeenFlags.Should().ContainSingle(x => x.Key == "buyout_test_mastery_gate_mastery_crop");
    }

    [Fact]
    public async Task BuyMasteryGate_BusinessGate_ChargesCashCost()
    {
        var user = Faker.Player();
        user.SetCash(150);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyMasteryGate(Context);
        var request = new BuyMasteryGateRequest { ItemName = "test_mastery_gate", GateName = "mastery_business" };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.Cash.Should().Be(70);
        player.SeenFlags.Should().ContainSingle(x => x.Key == "buyout_test_mastery_gate_mastery_business");
    }

    [Fact]
    public async Task BuyMasteryGate_AlreadyBought_DoesNotChargeAgain()
    {
        var user = Faker.Player();
        user.SetCash(150);
        user.SetSeenFlag("buyout_test_mastery_gate_mastery_crop");

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyMasteryGate(Context);
        var request = new BuyMasteryGateRequest { ItemName = "test_mastery_gate", GateName = "mastery_crop" };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.Cash.Should().Be(150);
        player.SeenFlags.Where(x => x.Key == "buyout_test_mastery_gate_mastery_crop").Should().ContainSingle();
    }

    [Fact]
    public async Task BuyMasteryGate_NotEnoughCash_ThrowsNotEnoughMoney()
    {
        var user = Faker.Player();
        user.SetCash(99);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyMasteryGate(Context);
        var request = new BuyMasteryGateRequest { ItemName = "test_mastery_gate", GateName = "mastery_crop" };

        var act = () => handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.NotEnoughMoney);
        user.Cash.Should().Be(99);
    }

    [Theory]
    [InlineData("test_mastery_gate", "inventory_gate")]
    [InlineData("test_mastery_gate", "unknown_gate")]
    [InlineData("unknown_item", "mastery_crop")]
    public async Task BuyMasteryGate_InvalidGate_ThrowsException(string itemName, string gateName)
    {
        var user = Faker.Player();
        user.SetCash(150);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new BuyMasteryGate(Context);
        var request = new BuyMasteryGateRequest { ItemName = itemName, GateName = gateName };

        var act = () => handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
        user.Cash.Should().Be(150);
    }

    [Fact]
    public async Task BuyMasteryGate_InvalidUser_ThrowsException()
    {
        var handler = new BuyMasteryGate(Context);
        var request = new BuyMasteryGateRequest { ItemName = "test_mastery_gate", GateName = "mastery_crop" };

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData("", "mastery_crop")]
    [InlineData("test_mastery_gate", "")]
    public void BuyMasteryGate_EmptyParams_FailsValidation(string itemName, string gateName)
    {
        var validator = new BuyMasteryGateValidator();
        var request = new BuyMasteryGateRequest { ItemName = itemName, GateName = gateName };

        var result = validator.TestValidate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void BuyMasteryGate_ParamsTooLong_FailsValidation()
    {
        var validator = new BuyMasteryGateValidator();
        var request = new BuyMasteryGateRequest { ItemName = Faker.Random.String2(65), GateName = Faker.Random.String2(33) };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ItemName);
        result.ShouldHaveValidationErrorFor(x => x.GateName);
    }
}
