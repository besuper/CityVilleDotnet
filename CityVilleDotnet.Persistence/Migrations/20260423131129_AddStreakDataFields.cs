using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStreakDataFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ActivationTime",
                table: "WorldObject",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InactiveTime",
                table: "WorldObject",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StreakLength",
                table: "WorldObject",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivationTime",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "InactiveTime",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "StreakLength",
                table: "WorldObject");
        }
    }
}
