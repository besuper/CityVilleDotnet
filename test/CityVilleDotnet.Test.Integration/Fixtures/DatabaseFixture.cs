using CityVilleDotnet.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace CityVilleDotnet.Test.Integration.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        await using var context = CreateDbContext();
        await context.Database.EnsureCreatedAsync();
    }

    public CityVilleDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<CityVilleDbContext>()
            .UseSqlServer(_container.GetConnectionString())
            .Options;

        return new CityVilleDbContext(options);
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>;