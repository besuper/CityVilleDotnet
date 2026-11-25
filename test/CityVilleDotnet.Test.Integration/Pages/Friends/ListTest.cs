using System.Security.Claims;
using AwesomeAssertions;
using CityVilleDotnet.Api.Pages.Friends;
using CityVilleDotnet.Api.Resources;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Factory.Player;
using CityVilleDotnet.Test.Integration.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace CityVilleDotnet.Test.Integration.Pages.Friends;

[Collection("Database")]
public class ListTest(DatabaseFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task List_OnGetCancel_PendingRequest_RemovesBothFriendships()
    {
        var (sender, target) = await SeedFriendRequest();

        var result = await CreatePage(sender).OnGetCancel(target.Username, TestContext.Current.CancellationToken);

        result.Should().BeOfType<RedirectToPageResult>().Which.PageName.Should().Be("/Friends/List");
        (await CountFriendships(sender, target)).Should().Be(0);
    }

    [Fact]
    public async Task List_OnGetRemove_Friend_RemovesBothFriendships()
    {
        var (sender, target) = await SeedFriendRequest();

        var result = await CreatePage(sender).OnGetRemove(target.Username, TestContext.Current.CancellationToken);

        result.Should().BeOfType<RedirectToPageResult>().Which.PageName.Should().Be("/Friends/List");
        (await CountFriendships(sender, target)).Should().Be(0);
    }

    private async Task<(Player Sender, Player Target)> SeedFriendRequest()
    {
        var sender = Faker.Player();
        var target = Faker.Player();
        sender.SendFriendRequest(target);

        await Context.AddRangeAsync([sender, target], TestContext.Current.CancellationToken);
        await Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        return (sender, target);
    }

    private Task<int> CountFriendships(Player a, Player b)
    {
        return Context.Set<Friend>().CountAsync(x =>
            (x.Player!.Id == a.Id && x.FriendPlayer!.Id == b.Id) || (x.Player!.Id == b.Id && x.FriendPlayer!.Id == a.Id),
            TestContext.Current.CancellationToken);
    }

    private ListModel CreatePage(Player currentPlayer)
    {
        var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(Context), Options.Create(new IdentityOptions()),
            new PasswordHasher<ApplicationUser>(), [], [], new UpperInvariantLookupNormalizer(), new IdentityErrorDescriber(), null!,
            NullLogger<UserManager<ApplicationUser>>.Instance);

        var localizer = new StringLocalizer<SharedResource>(new ResourceManagerStringLocalizerFactory(Options.Create(new LocalizationOptions()), NullLoggerFactory.Instance));

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, currentPlayer.AppUser!.Id)], "Test"))
        };

        return new ListModel(userManager, Context, localizer)
        {
            PageContext = new PageContext
            {
                HttpContext = httpContext,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            },
            TempData = new TempDataDictionary(httpContext, new NoTempDataProvider())
        };
    }

    private class NoTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context) => new Dictionary<string, object>();

        public void SaveTempData(HttpContext context, IDictionary<string, object> values)
        {
        }
    }
}
