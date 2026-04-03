using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveQuestsInPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quest_User_UserId",
                table: "Quest");
            
            migrationBuilder.Sql(@"
                UPDATE Quest
                SET UserId = p.Id
                FROM Quest q
                INNER JOIN [User] u ON u.Id = q.UserId
                INNER JOIN [Player] p ON p.Id = u.PlayerId
            ");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Quest",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Quest_UserId",
                table: "Quest",
                newName: "IX_Quest_PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quest_Player_PlayerId",
                table: "Quest",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quest_Player_PlayerId",
                table: "Quest");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Quest",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Quest_PlayerId",
                table: "Quest",
                newName: "IX_Quest_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quest_User_UserId",
                table: "Quest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
