using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFranchiseLocationWorldObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FranchiseLocationId",
                table: "WorldObject",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorldObject_FranchiseLocationId",
                table: "WorldObject",
                column: "FranchiseLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorldObject_FranchiseLocation_FranchiseLocationId",
                table: "WorldObject",
                column: "FranchiseLocationId",
                principalTable: "FranchiseLocation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorldObject_FranchiseLocation_FranchiseLocationId",
                table: "WorldObject");

            migrationBuilder.DropIndex(
                name: "IX_WorldObject_FranchiseLocationId",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "FranchiseLocationId",
                table: "WorldObject");
        }
    }
}
