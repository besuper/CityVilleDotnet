using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFranchises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Franchise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FranchiseType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    FranchiseName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    TimeLastCollected = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franchise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Franchise_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FranchiseLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StarRating = table.Column<int>(type: "int", nullable: false),
                    CommodityLeft = table.Column<int>(type: "int", nullable: false),
                    CommodityMax = table.Column<int>(type: "int", nullable: false),
                    CustomersServed = table.Column<int>(type: "int", nullable: false),
                    MoneyCollected = table.Column<int>(type: "int", nullable: false),
                    ObjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeLastCollected = table.Column<int>(type: "int", nullable: false),
                    TimeLastOperated = table.Column<int>(type: "int", nullable: false),
                    TimeLastSupplied = table.Column<int>(type: "int", nullable: false),
                    FranchiseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FranchiseLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FranchiseLocation_Franchise_FranchiseId",
                        column: x => x.FranchiseId,
                        principalTable: "Franchise",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Franchise_PlayerId",
                table: "Franchise",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_FranchiseLocation_FranchiseId",
                table: "FranchiseLocation",
                column: "FranchiseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FranchiseLocation");

            migrationBuilder.DropTable(
                name: "Franchise");
        }
    }
}
