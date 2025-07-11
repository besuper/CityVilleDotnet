using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Inventory_InventoryId",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Inventory_InventoryId",
                table: "Player");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Player_InventoryId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Player");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "InventoryItem",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItem_InventoryId",
                table: "InventoryItem",
                newName: "IX_InventoryItem_PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Player_PlayerId",
                table: "InventoryItem",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Player_PlayerId",
                table: "InventoryItem");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "InventoryItem",
                newName: "InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItem_PlayerId",
                table: "InventoryItem",
                newName: "IX_InventoryItem_InventoryId");

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryId",
                table: "Player",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_InventoryId",
                table: "Player",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Inventory_InventoryId",
                table: "InventoryItem",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Inventory_InventoryId",
                table: "Player",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id");
        }
    }
}
