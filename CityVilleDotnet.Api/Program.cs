using System.Globalization;
using System.IO.Compression;
using System.Threading.RateLimiting;
using CityVilleDotnet.Api.Common.Amf;
using CityVilleDotnet.Api.Features.Gateway.Endpoint;
using CityVilleDotnet.Common.Settings;
using CityVilleDotnet.Domain.Entities;
using CityVilleDotnet.Persistence;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using CityVilleDotnet.Api.Data;
using CityVilleDotnet.Api.Middleware;
using CityVilleDotnet.Common.Global;
using CityVilleDotnet.Common.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContextPool<CityVilleDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 2;

        options.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<CityVilleDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
});

builder.Services.AddRazorPages().AddViewLocalization();
builder.Services.AddFastEndpoints();
builder.Services.AddHttpContextAccessor();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}

builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.Configure<JsonOptions>(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = LocaleUtils.SUPPORTED_LOCALES.Select(x => new CultureInfo(x)).ToList();

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var executingAssembly = Assembly.GetExecutingAssembly();
var serviceTypes = executingAssembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AmfService)));

foreach (var type in serviceTypes)
{
    builder.Services.AddScoped(type);
}

GatewayService.InitializeHandlers(executingAssembly);

builder.Services.AddValidatorsFromAssembly(executingAssembly);

ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en");

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["text/xml", "application/xml"]);
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var path = context.Request.Path.Value ?? string.Empty;

        if (path.StartsWith("/assets", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".swf", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".js", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".css", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith("gateway.php", StringComparison.OrdinalIgnoreCase)
           )
        {
            return RateLimitPartition.GetNoLimiter(string.Empty);
        }

        var remoteIp = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault()
                       ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',', StringSplitOptions.TrimEntries).FirstOrDefault()
                       ?? context.Connection.RemoteIpAddress?.ToString()
                       ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(remoteIp, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 50,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        });
    });
});

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseExceptionHandler("/Error");

app.UseResponseCompression();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.Context.Request.Path.StartsWithSegments("/assets"))
        {
            ctx.Context.Response.Headers.CacheControl = "public, max-age=2592000";
        }
    }
});
app.UseMiddleware<FallbackAssetMiddleware>();
app.UseRateLimiter();
app.UseRouting();
app.MapGet("/", () => Results.Redirect("/Game"));
app.MapRazorPages();
app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();
app.UseFastEndpoints();

var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

StaticLogger.Configure(app.Services.GetRequiredService<ILoggerFactory>());

using var scope = app.Services.CreateScope();
await using var context = scope.ServiceProvider.GetRequiredService<CityVilleDbContext>();
await context.Database.MigrateAsync();

var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

ServerUtils.CheckRequiredFiles(logger);

GameSettingsManager.Instance.Initialize(scope.ServiceProvider.GetRequiredService<ILogger<GameSettingsManager>>());
QuestSettingsManager.Instance.Initialize(scope.ServiceProvider.GetRequiredService<ILogger<QuestSettingsManager>>());

await SamanthaSeeder.SeedAsync(app.Services);

logger.LogInformation("Version {InformationalVersion} loaded", Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion);

app.Run();