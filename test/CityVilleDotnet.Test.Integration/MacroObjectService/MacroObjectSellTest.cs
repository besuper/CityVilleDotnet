using AwesomeAssertions;
using CityVilleDotnet.Api.Services.MacroObjectService;
using CityVilleDotnet.Common.Enums;
using CityVilleDotnet.Common.Exceptions;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.World;
using CityVilleDotnet.Factory.WorldObject;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Test.Integration.MacroObjectService;

[Collection("Database")]
public class MacroObjectSellTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task MacroObjectSell_ParentSendsToInventory_StoresParentAndFlaggedChildren()
    {
        var house = Faker.WorldObject(itemName: "test_island_house", className: BuildingClassType.Residence, worldFlatId: 1);
        var background = Faker.WorldObject(itemName: "test_island_bg", className: BuildingClassType.BasePier, worldFlatId: 2);
        var kiosk = Faker.WorldObject(itemName: "test_island_kiosk", className: BuildingClassType.Decoration, worldFlatId: 3);
        var cottage = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, worldFlatId: 10);
        var world = Faker.World(objects: [house, background, kiosk, cottage]);
        world.CreateMacroObject("test_island_macro", "test_island_macro", [house, background, kiosk]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var goldBefore = player.Gold;
        var handler = new MacroObjectSell(Context);
        var request = new MacroObjectSellRequest { MacroObjectName = "test_island_macro_0", SoldObjects = [1, 2, 3] };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        player.Gold.Should().Be(goldBefore);
        player.CountInventoryItem("test_island_macro").Should().Be(1);
        player.CountInventoryItem("test_island_kiosk").Should().Be(1);
        player.CountInventoryItem("test_island_house").Should().Be(0);
        player.CountInventoryItem("test_island_bg").Should().Be(0);

        world.Objects.Should().ContainSingle().Which.WorldFlatId.Should().Be(10);
        world.Population.Should().Be(20);
        (await Context.Set<MacroObject>().AnyAsync(x => x.Name == "test_island_macro_0")).Should().BeFalse();
        (await Context.Set<WorldObject>().AnyAsync(x => x.Id == house.Id || x.Id == background.Id || x.Id == kiosk.Id)).Should().BeFalse();
    }

    [Fact]
    public async Task MacroObjectSell_ParentNotSendsToInventory_GivesChildrenSellPrice()
    {
        var house = Faker.WorldObject(itemName: "test_island_house", className: BuildingClassType.Residence, worldFlatId: 1);
        var shop = Faker.WorldObject(itemName: "test_island_shop", className: BuildingClassType.TimedBusiness, worldFlatId: 2);
        var background = Faker.WorldObject(itemName: "test_island_bg", className: BuildingClassType.BasePier, worldFlatId: 3);
        var tower = Faker.WorldObject(itemName: "test_island_tower", className: BuildingClassType.Municipal, worldFlatId: 4);
        var world = Faker.World(objects: [house, shop, background, tower]);
        world.CreateMacroObject("test_lagoon_macro", "test_lagoon_macro", [house, shop, background, tower]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var goldBefore = player.Gold;
        var handler = new MacroObjectSell(Context);
        var request = new MacroObjectSellRequest { MacroObjectName = "test_lagoon_macro_0", SoldObjects = [1, 2, 3, 4] };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        // house 79300 * 0.05 + shop 120 goods / 0.20 * 0.05 + background 0 + tower 79300 * 0.05
        player.Gold.Should().Be(goldBefore + 3965 + 30 + 0 + 3965);
        player.CountInventoryItem("test_lagoon_macro").Should().Be(0);

        world.Objects.Should().BeEmpty();
        world.MacroObjects.Should().BeEmpty();
    }

    [Fact]
    public async Task MacroObjectSell_ChildAlreadySentToInventory_IgnoresMissingObject()
    {
        var house = Faker.WorldObject(itemName: "test_island_house", className: BuildingClassType.Residence, worldFlatId: 1);
        var world = Faker.World(objects: [house]);
        world.CreateMacroObject("test_lagoon_macro", "test_lagoon_macro", [house]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var goldBefore = player.Gold;
        var handler = new MacroObjectSell(Context);
        // 2 was the kiosk, removed by its own TSendToInventory just before the macro object sell
        var request = new MacroObjectSellRequest { MacroObjectName = "test_lagoon_macro_0", SoldObjects = [1, 2] };

        var response = await handler.HandlePacket(request, player.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);
        player.Gold.Should().Be(goldBefore + 3965);
        world.Objects.Should().BeEmpty();
        world.MacroObjects.Should().BeEmpty();
    }

    [Fact]
    public async Task MacroObjectSell_ObjectOutsideMacroObject_ThrowsInvalidData()
    {
        var house = Faker.WorldObject(itemName: "test_island_house", className: BuildingClassType.Residence, worldFlatId: 1);
        var cottage = Faker.WorldObject(itemName: "res_cottage3", className: BuildingClassType.Residence, worldFlatId: 10);
        var world = Faker.World(objects: [house, cottage]);
        world.CreateMacroObject("test_island_macro", "test_island_macro", [house]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new MacroObjectSell(Context);
        var request = new MacroObjectSellRequest { MacroObjectName = "test_island_macro_0", SoldObjects = [1, 10] };

        var act = () => handler.HandlePacket(request, player.Id, CancellationToken.None);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidData);
    }

    [Fact]
    public async Task MacroObjectSell_UnknownMacroObject_ThrowsInvalidData()
    {
        var house = Faker.WorldObject(itemName: "test_island_house", className: BuildingClassType.Residence, worldFlatId: 1);
        var world = Faker.World(objects: [house]);
        world.CreateMacroObject("test_island_macro", "test_island_macro", [house]);
        var player = Faker.Player(world: world);

        await Context.AddAsync(player);
        await Context.SaveChangesAsync();

        var handler = new MacroObjectSell(Context);
        var request = new MacroObjectSellRequest { MacroObjectName = "test_island_macro_1", SoldObjects = [1] };

        var act = () => handler.HandlePacket(request, player.Id, CancellationToken.None);

        (await act.Should().ThrowAsync<DomainException>()).Which.Reason.Should().Be(GameErrorType.InvalidData);
    }
}
