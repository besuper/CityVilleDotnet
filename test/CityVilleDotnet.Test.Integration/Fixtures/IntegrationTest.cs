using Bogus;
using CityVilleDotnet.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace CityVilleDotnet.Test.Integration.Fixtures;

[Collection("Database")]
public abstract class IntegrationTest(DatabaseFixture fixture) : IAsyncLifetime
{
    protected readonly DatabaseFixture Fixture = fixture;
    protected readonly Faker Faker = new();
    protected CityVilleDbContext Context = null!;
    private IDbContextTransaction _transaction = null!;

    public async ValueTask InitializeAsync()
    {
        Context = Fixture.CreateDbContext();
        _transaction = await Context.Database.BeginTransactionAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
        await Context.DisposeAsync();
    }
}