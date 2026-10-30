using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class SetSeenFlagIntTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task SetSeenFlagInt_NewFlag_AddsValue()
    {
        var user = Faker.Player();

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new SetSeenFlagInt(Context);
        var request = new SetSeenFlagIntRequest { FlagName = "test_announcement", Value = 2 };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_announcement").Which.Value.Should().Be(2);
    }

    [Fact]
    public async Task SetSeenFlagInt_ExistingFlag_UpdatesValueAndKeepsOtherFlags()
    {
        var user = Faker.Player();
        user.SetSeenFlagInt("test_announcement", 1);
        user.SetSeenFlag("test_other");

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new SetSeenFlagInt(Context);
        var request = new SetSeenFlagIntRequest { FlagName = "test_announcement", Value = 0 };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.SeenFlags.Should().HaveCount(2);
        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_announcement").Which.Value.Should().Be(0);
        player.SeenFlags.Should().ContainSingle(x => x.Key == "test_other").Which.Value.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void SetSeenFlagInt_EmptyFlagName_FailsValidation(string? flagName)
    {
        var validator = new SetSeenFlagIntValidator();
        var request = new SetSeenFlagIntRequest { FlagName = flagName!, Value = 1 };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FlagName);
    }

    [Fact]
    public void SetSeenFlagInt_FlagNameTooLong_FailsValidation()
    {
        var validator = new SetSeenFlagIntValidator();
        var request = new SetSeenFlagIntRequest { FlagName = Faker.Random.String2(65), Value = 1 };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FlagName);
    }

    [Fact]
    public async Task SetSeenFlagInt_InvalidUser_ThrowsException()
    {
        var handler = new SetSeenFlagInt(Context);
        var request = new SetSeenFlagIntRequest { FlagName = "test_flag", Value = 1 };

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }
}
