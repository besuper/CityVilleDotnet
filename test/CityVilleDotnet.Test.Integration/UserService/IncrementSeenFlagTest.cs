using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class IncrementSeenFlagTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task IncrementSeenFlag_NewFlag_StartsAtOne()
    {
        var user = Faker.Player();

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new IncrementSeenFlag(Context);
        var request = new IncrementSeenFlagRequest { FlagName = "test_counter" };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_counter").Which.Value.Should().Be(1);
    }

    [Fact]
    public async Task IncrementSeenFlag_ExistingIntFlag_AddsOneAndKeepsOtherFlags()
    {
        var user = Faker.Player();
        user.SetSeenFlagInt("test_counter", 3);
        user.SetSeenFlag("test_other");

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new IncrementSeenFlag(Context);
        var request = new IncrementSeenFlagRequest { FlagName = "test_counter" };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.SeenFlags.Should().HaveCount(2);
        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_counter").Which.Value.Should().Be(4);
        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_other").Which.Value.Should().BeNull();
    }

    [Fact]
    public async Task IncrementSeenFlag_BooleanFlag_BecomesTwo()
    {
        var user = Faker.Player();
        user.SetSeenFlag("test_counter");

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new IncrementSeenFlag(Context);
        var request = new IncrementSeenFlagRequest { FlagName = "test_counter" };

        await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_counter").Which.Value.Should().Be(2);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void IncrementSeenFlag_EmptyFlagName_FailsValidation(string? flagName)
    {
        var validator = new IncrementSeenFlagValidator();
        var request = new IncrementSeenFlagRequest { FlagName = flagName! };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FlagName);
    }

    [Fact]
    public void IncrementSeenFlag_FlagNameTooLong_FailsValidation()
    {
        var validator = new IncrementSeenFlagValidator();
        var request = new IncrementSeenFlagRequest { FlagName = Faker.Random.String2(65) };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FlagName);
    }

    [Fact]
    public async Task IncrementSeenFlag_InvalidUser_ThrowsException()
    {
        var handler = new IncrementSeenFlag(Context);
        var request = new IncrementSeenFlagRequest { FlagName = "test_flag" };

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }
}
