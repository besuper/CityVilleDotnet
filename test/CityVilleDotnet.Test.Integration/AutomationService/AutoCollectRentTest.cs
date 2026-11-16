using AwesomeAssertions;
using CityVilleDotnet.Api.Services.AutomationService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Common.Utils;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CityVilleDotnet.Test.Integration.AutomationService;

// See test_auto_res in TestData/gameSettings.xml and qm_test_auto_harvest in TestData/questSettings.xml
[Collection("Database")]
public class AutoCollectRentTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const long OneHourMs = 3_600_000;

    private AutoCollectRent CreateHandler()
    {
        return new AutoCollectRent(Context, new ConfigurationBuilder().Build());
    }

    [Fact]
    public async Task AutoCollectRent_GrownResidences_HarvestsAndPaysGasAndEnergy()
    {
        var first = Faker.WorldObject(itemName: "test_auto_res", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - OneHourMs, worldFlatId: 1);
        var second = Faker.WorldObject(itemName: "test_auto_res", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - OneHourMs, worldFlatId: 2);
        var player = Faker.Player(world: Faker.World(objects: [first, second]));
        player.SetLevel(2);
        var quest = Faker.Quest(name: "qm_test_auto_harvest", length: 2);
        player.Quests.Add(quest);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var energyBefore = player.Energy;
        var timeBefore = ServerUtils.GetCurrentTime();

        var response = await CreateHandler().HandlePacket(new AutoCollectRentRequest { MapResourceIds = [1, 2] }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var residences = await Context.Set<WorldObject>().Where(x => x.Id == first.Id || x.Id == second.Id).ToListAsync(TestContext.Current.CancellationToken);

        residences.Should().AllSatisfy(x => x.PlantTime.Should().BeGreaterThanOrEqualTo(timeBefore));
        player.Gas.Should().Be(10);
        player.Energy.Should().Be(energyBefore - 4);
        quest.Progress.Should().Equal(2, 2);
    }

    [Fact]
    public async Task AutoCollectRent_NotGrownResidence_SkipsHarvestButPaysGas()
    {
        var plantTime = ServerUtils.GetCurrentTime();
        var residence = Faker.WorldObject(itemName: "test_auto_res", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: plantTime, worldFlatId: 1);
        var player = Faker.Player(world: Faker.World(objects: [residence]));
        player.SetLevel(2);

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var response = await CreateHandler().HandlePacket(new AutoCollectRentRequest { MapResourceIds = [1] }, player.Id, TestContext.Current.CancellationToken);

        response["errorType"].Should().Be(0);

        var updatedResidence = await Context.Set<WorldObject>().FirstAsync(x => x.Id == residence.Id, TestContext.Current.CancellationToken);

        updatedResidence.PlantTime.Should().Be(plantTime);
        player.Gas.Should().Be(11);
    }

    [Fact]
    public async Task AutoCollectRent_BelowRequiredLevel_ThrowsInvalidState()
    {
        var residence = Faker.WorldObject(itemName: "test_auto_res", className: BuildingClassType.Residence, state: WorldObjectState.Planted, plantTime: ServerUtils.GetCurrentTime() - OneHourMs, worldFlatId: 1);
        var player = Faker.Player(world: Faker.World(objects: [residence]));

        await Context.AddAsync(player, TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var act = () => CreateHandler().HandlePacket(new AutoCollectRentRequest { MapResourceIds = [1] }, player.Id, TestContext.Current.CancellationToken);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidState);
    }

    [Fact]
    public void AutoCollectRentRequestValidator_DuplicateIds_HasError()
    {
        var result = new AutoCollectRentRequestValidator().TestValidate(new AutoCollectRentRequest { MapResourceIds = [1, 1] });

        result.ShouldHaveValidationErrorFor(x => x.MapResourceIds);
    }
}
