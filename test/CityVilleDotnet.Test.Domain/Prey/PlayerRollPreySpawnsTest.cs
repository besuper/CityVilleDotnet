using AwesomeAssertions;
using Bogus;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Common.Settings.GameSettings;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.PreyGroup;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;

namespace CityVilleDotnet.Test.Domain.Prey;

// See test_prey, test_prey_gated and test_prey_z in TestData/gameSettings.xml
[Collection("Domain")]
public class PlayerRollPreySpawnsTest
{
    private static readonly Dictionary<string, int> NoExperiments = [];
    private static readonly Dictionary<string, int> GatedExperiments = new() { ["test_prey_mode_exp"] = 2 };

    private static GameItem Item(string name) => GameSettingsManager.Instance.GetItem(name)!;

    private static int? RollCount(CityVilleDotnet.Domain.Entities.Player player, string feature)
    {
        return player.FeatureRollCounters.SingleOrDefault(x => x.Feature == feature)?.Count;
    }

    private static (CityVilleDotnet.Domain.Entities.Player Player, CityVilleDotnet.Domain.Entities.World World) CreatePlayer(Faker faker, bool withHub, List<int> preys, WorldType worldType = WorldType.Main)
    {
        List<CityVilleDotnet.Domain.Entities.WorldObject> objects = withHub ? [faker.WorldObject(itemName: "test_prey_hub_2", className: BuildingClassType.Municipal)] : [];
        var world = faker.World(objects: objects, type: worldType);
        var player = faker.Player(world: world);
        player.PreyGroups.Add(faker.PreyGroup(groupId: "test_prey", preys: preys));

        return (player, world);
    }

    [Fact]
    public void Player_RollPreySpawns_SpawnWithQueuedPrey_StopsBeforeNextGroup()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: true, preys: [1]);

        player.RollPreySpawns(world, Item("test_prey_shop"), NoExperiments);

        RollCount(player, "test_prey").Should().Be(2);
        RollCount(player, "test_prey_z").Should().BeNull();
    }

    [Fact]
    public void Player_RollPreySpawns_SpawnWithEmptyQueue_RollsNextGroup()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: true, preys: []);

        player.RollPreySpawns(world, Item("test_prey_shop"), NoExperiments);

        RollCount(player, "test_prey").Should().Be(2);
        RollCount(player, "test_prey_z").Should().Be(1);
    }

    [Fact]
    public void Player_RollPreySpawns_HubsLocked_SkipsGroup()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: false, preys: [1]);

        player.RollPreySpawns(world, Item("test_prey_shop"), NoExperiments);

        RollCount(player, "test_prey").Should().BeNull();
        RollCount(player, "test_prey_z").Should().Be(1);
    }

    [Fact]
    public void Player_RollPreySpawns_SourceOfAnotherWorld_IsSkipped()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: true, preys: [1], worldType: WorldType.Downtown);

        player.RollPreySpawns(world, Item("test_prey_shop"), NoExperiments);

        RollCount(player, "test_prey").Should().Be(1);
        RollCount(player, "test_prey_z").Should().Be(1);
    }

    [Fact]
    public void Player_RollPreySpawns_NoMatchingSource_DoesNotRoll()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: true, preys: [1]);

        player.RollPreySpawns(world, Item("res_cottage3"), NoExperiments);

        player.FeatureRollCounters.Should().BeEmpty();
    }

    [Fact]
    public void Player_RollPreySpawns_ModeConditionsMet_Rolls()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: false, preys: []);
        player.SetLevel(10);

        player.RollPreySpawns(world, Item("test_prey_gated_deco"), GatedExperiments);

        RollCount(player, "test_prey_gated").Should().Be(1);
    }

    [Fact]
    public void Player_RollPreySpawns_PlayerLevelTooLow_DoesNotRoll()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: false, preys: []);
        player.SetLevel(9);

        player.RollPreySpawns(world, Item("test_prey_gated_deco"), GatedExperiments);

        RollCount(player, "test_prey_gated").Should().BeNull();
    }

    [Fact]
    public void Player_RollPreySpawns_ModeExperimentNotInVariant_DoesNotRoll()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: false, preys: []);
        player.SetLevel(10);

        player.RollPreySpawns(world, Item("test_prey_gated_deco"), new Dictionary<string, int> { ["test_prey_mode_exp"] = 1 });

        RollCount(player, "test_prey_gated").Should().BeNull();
    }

    [Fact]
    public void Player_RollPreySpawns_ModeWorldMismatch_DoesNotRoll()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: false, preys: [], worldType: WorldType.Downtown);
        player.SetLevel(10);

        player.RollPreySpawns(world, Item("test_prey_gated_deco"), GatedExperiments);

        RollCount(player, "test_prey_gated").Should().BeNull();
    }

    [Fact]
    public void Player_RollPreySpawns_SourceExperimentInVariant_Rolls()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: false, preys: []);
        player.SetLevel(10);

        player.RollPreySpawns(world, Item("test_prey_exp_deco"), new Dictionary<string, int> { ["test_prey_mode_exp"] = 2, ["test_prey_source_exp"] = 1 });

        RollCount(player, "test_prey_gated").Should().Be(1);
    }

    [Fact]
    public void Player_RollPreySpawns_SourceExperimentNotInVariant_DoesNotRoll()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: false, preys: []);
        player.SetLevel(10);

        player.RollPreySpawns(world, Item("test_prey_exp_deco"), GatedExperiments);

        RollCount(player, "test_prey_gated").Should().BeNull();
    }

    [Fact]
    public void Player_RollPreySpawns_CounterKeepsGoing_AcrossHarvests()
    {
        var faker = new Faker();
        var (player, world) = CreatePlayer(faker, withHub: false, preys: []);

        player.RollPreySpawns(world, Item("test_prey_shop"), NoExperiments);
        player.RollPreySpawns(world, Item("test_prey_shop"), NoExperiments);

        RollCount(player, "test_prey_z").Should().Be(2);
    }
}
