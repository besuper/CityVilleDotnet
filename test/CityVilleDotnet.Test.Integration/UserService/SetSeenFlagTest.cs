using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.User;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class SetSeenFlagTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task SetSeenFlag_ValidFlag()
    {
        var user = Faker.User();

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new SetSeenFlag(Context, NullLogger<SetSeenFlag>.Instance);
        var request = new SetSeenFlagRequest { FlagName = "tutorial_complete" };

        var response = await handler.HandlePacket(request, user.UserId, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Player!.Id);

        player.SeenFlags.Should().ContainSingle(x => x.Key == "tutorial_complete");
    }

    [Fact]
    public async Task SetSeenFlag_DuplicateFlag_DoesNotAddTwice()
    {
        var user = Faker.User();
        user.GetPlayer().SetSeenFlag("already_seen");

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new SetSeenFlag(Context, NullLogger<SetSeenFlag>.Instance);
        var request = new SetSeenFlagRequest { FlagName = "already_seen" };

        var response = await handler.HandlePacket(request, user.UserId, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.SeenFlags)
            .FirstAsync(x => x.Id == user.Player!.Id);

        player.SeenFlags.Where(x => x.Key == "already_seen").Should().ContainSingle();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void SetSeenFlag_EmptyFlagName_FailsValidation(string? flagName)
    {
        var validator = new SetSeenFlagValidator();
        var request = new SetSeenFlagRequest { FlagName = flagName! };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FlagName);
    }

    [Fact]
    public void SetSeenFlag_FlagNameTooLong_FailsValidation()
    {
        var validator = new SetSeenFlagValidator();
        var request = new SetSeenFlagRequest { FlagName = Faker.Random.String2(65) };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FlagName);
    }

    [Fact]
    public async Task SetSeenFlag_InvalidUser_ThrowsException()
    {
        var handler = new SetSeenFlag(Context, NullLogger<SetSeenFlag>.Instance);
        var request = new SetSeenFlagRequest { FlagName = "some_flag" };

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }
}