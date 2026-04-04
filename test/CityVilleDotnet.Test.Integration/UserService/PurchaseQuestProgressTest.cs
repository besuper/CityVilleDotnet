using AwesomeAssertions;
using CityVilleDotnet.Api.Services.UserService;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Domain.Enums;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Factory.Quest;
using CityVilleDotnet.Test.Integration.Fixtures;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CityVilleDotnet.Test.Integration.UserService;

[Collection("Database")]
public class PurchaseQuestProgressTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    private const string QuestName = "qm_test_quest";

    [Fact]
    public async Task PurchaseQuestProgress_ValidPurchase()
    {
        var user = Faker.Player();
        user.Quests.Add(Faker.Quest(name: QuestName, length: 3));
        user.SetCash(100);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new PurchaseQuestProgress(Context, NullLogger<PurchaseQuestProgress>.Instance);
        var request = new PurchaseQuestProgressRequest { QuestName = QuestName, TaskIndex = 2 };

        var response = await handler.HandlePacket(request, user.Id, CancellationToken.None);

        response["errorType"].Should().Be(0);

        var player = await Context.Set<Player>().FirstAsync(x => x.Id == user.Id);
        player.Cash.Should().Be(50);

        var updatedQuest = await Context.Set<Quest>().FirstAsync(x => x.Name == QuestName);
        updatedQuest.Purchased[2].Should().Be(500);
    }

    [Fact]
    public async Task PurchaseQuestProgress_NotEnoughCash()
    {
        var user = Faker.Player();
        user.Quests.Add(Faker.Quest(name: QuestName, length: 3));
        user.SetCash(10);

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new PurchaseQuestProgress(Context, NullLogger<PurchaseQuestProgress>.Instance);
        var request = new PurchaseQuestProgressRequest { QuestName = QuestName, TaskIndex = 2 };

        var response = await handler.HandlePacket(request, user.Id, CancellationToken.None);

        response["errorType"].Should().Be((int)GameErrorType.NotEnoughMoney);

        var player = await Context.Set<Player>().FirstAsync(x => x.Id == user.Id);
        player.Cash.Should().Be(10);
    }

    [Fact]
    public async Task PurchaseQuestProgress_QuestNotFound()
    {
        var user = Faker.Player();

        await Context.AddAsync(user);
        await Context.SaveChangesAsync();

        var handler = new PurchaseQuestProgress(Context, NullLogger<PurchaseQuestProgress>.Instance);
        var request = new PurchaseQuestProgressRequest { QuestName = "nonexistent_quest", TaskIndex = 2 };

        var act = () => handler.HandlePacket(request, user.Id, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task PurchaseQuestProgress_InvalidUser()
    {
        var handler = new PurchaseQuestProgress(Context, NullLogger<PurchaseQuestProgress>.Instance);
        var request = new PurchaseQuestProgressRequest { QuestName = QuestName, TaskIndex = 2 };

        var act = () => handler.HandlePacket(request, Guid.NewGuid(), CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void PurchaseQuestProgress_EmptyQuestName(string? questName)
    {
        var validator = new PurchaseQuestProgressValidator();
        var request = new PurchaseQuestProgressRequest { QuestName = questName! };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.QuestName);
    }

    [Fact]
    public void PurchaseQuestProgress_QuestNameTooLong()
    {
        var validator = new PurchaseQuestProgressValidator();
        var request = new PurchaseQuestProgressRequest { QuestName = Faker.Random.String2(65) };

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.QuestName);
    }
}