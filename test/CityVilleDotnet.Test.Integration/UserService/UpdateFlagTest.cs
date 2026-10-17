using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class UpdateFlagTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private static UpdateFlagRequest CreateRequest(string name, int value)
    {
        return new UpdateFlagRequest { Params = new UpdateFlagParams { Name = name, Value = value } };
    }

    [Fact]
    public async Task UpdateFlag_NewFlag_AddsFlag()
    {
        var user = Faker.Player();

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpdateFlag(Context);

        var response = await handler.HandlePacket(CreateRequest("double_warehouse_check", -2), user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.Flags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.Flags.Should().ContainSingle(x => x.Name == "double_warehouse_check" && x.Value == -2);
    }

    [Fact]
    public async Task UpdateFlag_ExistingFlag_UpdatesValueAndKeepsOtherFlags()
    {
        var user = Faker.Player();
        user.SetFlag("check_goods_storage", 1);
        user.SetFlag("other_flag", 7);

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpdateFlag(Context);

        var response = await handler.HandlePacket(CreateRequest("check_goods_storage", 0), user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.Flags)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.Flags.Should().HaveCount(2);
        player.Flags.Should().ContainSingle(x => x.Name == "check_goods_storage").Which.Value.Should().Be(0);
        player.Flags.Should().ContainSingle(x => x.Name == "other_flag").Which.Value.Should().Be(7);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void UpdateFlag_EmptyName_FailsValidation(string? name)
    {
        var validator = new UpdateFlagValidator();

        var result = validator.TestValidate(CreateRequest(name!, 1));

        result.ShouldHaveValidationErrorFor(x => x.Params.Name);
    }

    [Fact]
    public void UpdateFlag_NameTooLong_FailsValidation()
    {
        var validator = new UpdateFlagValidator();

        var result = validator.TestValidate(CreateRequest(Faker.Random.String2(65), 1));

        result.ShouldHaveValidationErrorFor(x => x.Params.Name);
    }

    [Fact]
    public async Task UpdateFlag_InvalidUser_ThrowsException()
    {
        var handler = new UpdateFlag(Context);

        var act = () => handler.HandlePacket(CreateRequest("some_flag", 1), Guid.NewGuid(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }
}
