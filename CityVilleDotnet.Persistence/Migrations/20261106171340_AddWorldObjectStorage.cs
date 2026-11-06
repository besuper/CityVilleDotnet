using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorldObjectStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StorageSlotIndex",
                table: "WorldObject",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoredInId",
                table: "WorldObject",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageSlotIndex",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "StoredInId",
                table: "WorldObject");
        }
    }
}
