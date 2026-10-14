using AwesomeAssertions;
using CityVilleDotnet.Api.Services.FranchiseService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.FranchiseService;

[Collection("Database")]
public class UpdateFranchiseNameTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task UpdateFranchiseName_NewFranchise_CreatesFranchise()
    {
        var user = Faker.Player();

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpdateFranchiseName(Context);
        var request = new UpdateFranchiseNameRequest { FranchiseType = "bus_bakery", FranchiseName = "My Bakery" };

        var response = await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>()
            .Include(x => x.Franchises)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.Franchises.Should().ContainSingle(x => x.FranchiseType == "bus_bakery" && x.FranchiseName == "My Bakery");
    }

    [Fact]
    public async Task UpdateFranchiseName_ExistingFranchise_RenamesFranchise()
    {
        var user = Faker.Player();
        user.UpdateFranchiseName("bus_bakery", "Old Name");

        await Context.AddAsync(user, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new UpdateFranchiseName(Context);
        var request = new UpdateFranchiseNameRequest { FranchiseType = "bus_bakery", FranchiseName = "New Name" };

        await handler.HandlePacket(request, user.Id, TestContext.Current.CancellationToken);

        var player = await Context.Set<Player>()
            .Include(x => x.Franchises)
            .FirstAsync(x => x.Id == user.Id, TestContext.Current.CancellationToken);

        player.Franchises.Should().ContainSingle(x => x.FranchiseType == "bus_bakery" && x.FranchiseName == "New Name");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void UpdateFranchiseName_EmptyFranchiseType_FailsValidation(string? franchiseType)
    {
        var validator = new UpdateFranchiseNameValidator();
        var request = new UpdateFranchiseNameRequest { FranchiseType = franchiseType!, FranchiseName = "My Bakery" };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FranchiseType);
    }

    [Fact]
    public void UpdateFranchiseName_FranchiseTypeTooLong_FailsValidation()
    {
        var validator = new UpdateFranchiseNameValidator();
        var request = new UpdateFranchiseNameRequest { FranchiseType = Faker.Random.String2(65), FranchiseName = "My Bakery" };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FranchiseType);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void UpdateFranchiseName_EmptyFranchiseName_FailsValidation(string? franchiseName)
    {
        var validator = new UpdateFranchiseNameValidator();
        var request = new UpdateFranchiseNameRequest { FranchiseType = "bus_bakery", FranchiseName = franchiseName! };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FranchiseName);
    }

    [Fact]
    public void UpdateFranchiseName_FranchiseNameTooLong_FailsValidation()
    {
        var validator = new UpdateFranchiseNameValidator();
        var request = new UpdateFranchiseNameRequest { FranchiseType = "bus_bakery", FranchiseName = Faker.Random.String2(31) };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.FranchiseName);
    }

    [Fact]
    public async Task UpdateFranchiseName_InvalidUser_ThrowsException()
    {
        var handler = new UpdateFranchiseName(Context);
        var request = new UpdateFranchiseNameRequest { FranchiseType = "bus_bakery", FranchiseName = "My Bakery" };

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<Exception>();
    }
}
