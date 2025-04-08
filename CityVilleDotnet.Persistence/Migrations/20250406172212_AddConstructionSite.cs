using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConstructionSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Builds",
                table: "WorldObject",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinishedBuilds",
                table: "WorldObject",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "WorldObject",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetBuildingClass",
                table: "WorldObject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetBuildingName",
                table: "WorldObject",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Builds",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "FinishedBuilds",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "TargetBuildingClass",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "TargetBuildingName",
                table: "WorldObject");
        }
    }
}
