using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveAppUserInPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AspNetUsers_AppUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_AppUserId",
                table: "User");
            
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Player",
                type: "nvarchar(450)",
                nullable: true);
            
            migrationBuilder.Sql(@"
                UPDATE p 
                SET p.AppUserId = u.AppUserId
                FROM Player p
                INNER JOIN [User] u ON u.PlayerId = p.Id
            ");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Player_AppUserId",
                table: "Player",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_AspNetUsers_AppUserId",
                table: "Player",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_AspNetUsers_AppUserId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_AppUserId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Player");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_AppUserId",
                table: "User",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AspNetUsers_AppUserId",
                table: "User",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
