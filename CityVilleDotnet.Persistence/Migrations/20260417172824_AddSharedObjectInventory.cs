using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSharedObjectInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoredObjectId",
                table: "InventoryItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_StoredObjectId",
                table: "InventoryItem",
                column: "StoredObjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_WorldObject_StoredObjectId",
                table: "InventoryItem",
                column: "StoredObjectId",
                principalTable: "WorldObject",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_WorldObject_StoredObjectId",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_StoredObjectId",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "StoredObjectId",
                table: "InventoryItem");
        }
    }
}
