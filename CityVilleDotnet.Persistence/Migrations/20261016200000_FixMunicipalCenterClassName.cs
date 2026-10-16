using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    [DbContext(typeof(CityVilleDbContext))]
    [Migration("20261016200000_FixMunicipalCenterClassName")]
    public partial class FixMunicipalCenterClassName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [WorldObject] SET [ClassName] = 30 WHERE [ClassName] = 67");
            migrationBuilder.Sql("UPDATE [WorldObject] SET [TargetBuildingClass] = 30 WHERE [TargetBuildingClass] = 67");
            migrationBuilder.Sql("UPDATE [WorldObject] SET [ClassName] = 24 WHERE [ClassName] = 44");
            migrationBuilder.Sql("UPDATE [WorldObject] SET [TargetBuildingClass] = 24 WHERE [TargetBuildingClass] = 44");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
