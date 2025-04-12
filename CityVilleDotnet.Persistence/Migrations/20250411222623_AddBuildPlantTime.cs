using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildPlantTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BuildTime",
                table: "WorldObject",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PlantTime",
                table: "WorldObject",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildTime",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "PlantTime",
                table: "WorldObject");
        }
    }
}
