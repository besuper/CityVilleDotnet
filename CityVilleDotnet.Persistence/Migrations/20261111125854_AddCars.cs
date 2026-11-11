using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations;

/// <inheritdoc />
public partial class _20261111125854_AddCars : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "CarCraftEnabled",
            table: "Player",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "CarsEnabled",
            table: "Player",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.CreateTable(
            name: "CarItem",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Amount = table.Column<int>(type: "int", nullable: false),
                PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CarItem", x => x.Id);
                table.ForeignKey(
                    name: "FK_CarItem_Player_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Player",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CraftingPartItem",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Amount = table.Column<int>(type: "int", nullable: false),
                PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CraftingPartItem", x => x.Id);
                table.ForeignKey(
                    name: "FK_CraftingPartItem_Player_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Player",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CarItem_PlayerId",
            table: "CarItem",
            column: "PlayerId");

        migrationBuilder.CreateIndex(
            name: "IX_CraftingPartItem_PlayerId",
            table: "CraftingPartItem",
            column: "PlayerId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CarItem");

        migrationBuilder.DropTable(
            name: "CraftingPartItem");

        migrationBuilder.DropColumn(
            name: "CarCraftEnabled",
            table: "Player");

        migrationBuilder.DropColumn(
            name: "CarsEnabled",
            table: "Player");
    }
}
