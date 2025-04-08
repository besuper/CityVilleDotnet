using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace CityVilleDotnet.Persistence;

public class CityVilleDbContextFactory : IDesignTimeDbContextFactory<CityVilleDbContext>
{
    public CityVilleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CityVilleDbContext>();

        return new CityVilleDbContext(optionsBuilder.Options);
    }
}