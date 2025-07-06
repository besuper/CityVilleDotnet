using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCommodities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Commodities_CommoditiesId",
                table: "Player");

            migrationBuilder.DropTable(
                name: "Commodities");

            migrationBuilder.DropIndex(
                name: "IX_Player_CommoditiesId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "CommoditiesId",
                table: "Player");

            migrationBuilder.AddColumn<int>(
                name: "Goods",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Goods",
                table: "Player");

            migrationBuilder.AddColumn<Guid>(
                name: "CommoditiesId",
                table: "Player",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Commodities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Storage_Goods = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commodities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_CommoditiesId",
                table: "Player",
                column: "CommoditiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Commodities_CommoditiesId",
                table: "Player",
                column: "CommoditiesId",
                principalTable: "Commodities",
                principalColumn: "Id");
        }
    }
}
