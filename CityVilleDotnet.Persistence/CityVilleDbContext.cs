using CityVilleDotnet.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CityVilleDotnet.Persistence;

public class CityVilleDbContext : IdentityDbContext<ApplicationUser>
{
    public CityVilleDbContext(DbContextOptions<CityVilleDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();

        base.OnConfiguring(optionsBuilder);
    }
}
