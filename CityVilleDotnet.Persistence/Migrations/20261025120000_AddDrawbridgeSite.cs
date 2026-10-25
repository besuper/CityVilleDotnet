using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    [DbContext(typeof(CityVilleDbContext))]
    [Migration("20261025120000_AddDrawbridgeSite")]
    public partial class AddDrawbridgeSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO [WorldObject] ([ItemName], [ClassName], [Deleted], [TempId], [BuildTime], [PlantTime], [State], [Direction], [X], [Y], [Z], [WorldFlatId],
                    [TargetBuildingClass], [TargetBuildingName], [Stage], [FinishedBuilds], [Builds], [RequiredStages], [CurrentState], [NeverOpened],
                    [StreakLength], [EnergyModifier], [GivenFreeItem], [WorldId])
                SELECT 'drawbridge_construction', 7, 0, -1, DATEDIFF_BIG(MILLISECOND, '1970-01-01', SYSUTCDATETIME()), DATEDIFF_BIG(MILLISECOND, '1970-01-01', SYSUTCDATETIME()),
                    2, 0, 56, -93, 0, w.[NextBuildingId], 68, 'mun_drawbridge', 0, 0, 0, 0, 0, 1, 0, 0, 0, w.[Id]
                FROM [World] w
                WHERE w.[Type] = 0 AND NOT EXISTS (
                    SELECT 1 FROM [WorldObject] o
                    WHERE o.[WorldId] = w.[Id] AND (o.[ItemName] = 'mun_drawbridge' OR o.[TargetBuildingName] = 'mun_drawbridge'))
                """);

            migrationBuilder.Sql("""
                UPDATE w SET [NextBuildingId] = w.[NextBuildingId] + 1
                FROM [World] w
                JOIN [WorldObject] o ON o.[WorldId] = w.[Id] AND o.[ItemName] = 'drawbridge_construction' AND o.[WorldFlatId] = w.[NextBuildingId]
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
