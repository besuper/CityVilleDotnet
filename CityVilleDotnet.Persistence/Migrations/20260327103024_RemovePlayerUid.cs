using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovePlayerUid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Player_Uid",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Uid",
                table: "Player");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Uid",
                table: "Player",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Uid",
                table: "Player",
                column: "Uid");
        }
    }
}
