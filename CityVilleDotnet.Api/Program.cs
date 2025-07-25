using CityVilleDotnet.Api.Common.Amf;
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
using CityVilleDotnet.Common.Global;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<CityVilleDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

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

builder.Services.AddRazorPages();
builder.Services.AddFastEndpoints();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}

builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.Configure<JsonOptions>(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

var serviceTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AmfService)));

foreach (var type in serviceTypes)
{
    builder.Services.AddScoped(type);
}

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseStaticFiles();
app.MapRazorPages();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();
app.UseFastEndpoints();

StaticLogger.Configure(app.Services.GetRequiredService<ILoggerFactory>());

using var scope = app.Services.CreateScope();
await using var context = scope.ServiceProvider.GetRequiredService<CityVilleDbContext>();
await context.Database.MigrateAsync();

GameSettingsManager.Instance.Initialize(scope.ServiceProvider.GetRequiredService<ILogger<GameSettingsManager>>());
QuestSettingsManager.Instance.Initialize(scope.ServiceProvider.GetRequiredService<ILogger<QuestSettingsManager>>());

app.Run();