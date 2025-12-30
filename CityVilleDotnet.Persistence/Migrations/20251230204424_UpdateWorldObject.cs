using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorldObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "WorldObject",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequiredStages",
                table: "WorldObject",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "RequiredStages",
                table: "WorldObject");
        }
    }
}
