using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveFriendsInPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_User_FriendUserId",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_User_UserId",
                table: "Friend");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Friend",
                newName: "PlayerId");

            migrationBuilder.RenameColumn(
                name: "FriendUserId",
                table: "Friend",
                newName: "FriendPlayerId");
            
            migrationBuilder.Sql(@"
                UPDATE f
                SET f.PlayerId = p.Id
                FROM Friend f
                INNER JOIN [User] u ON u.Id = f.PlayerId
                INNER JOIN Player p ON p.Id = u.PlayerId
            ");

            migrationBuilder.Sql(@"
                UPDATE f
                SET f.FriendPlayerId = p.Id
                FROM Friend f
                INNER JOIN [User] u ON u.Id = f.FriendPlayerId
                INNER JOIN Player p ON p.Id = u.PlayerId
            ");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_UserId",
                table: "Friend",
                newName: "IX_Friend_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_FriendUserId",
                table: "Friend",
                newName: "IX_Friend_FriendPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Player_FriendPlayerId",
                table: "Friend",
                column: "FriendPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Player_PlayerId",
                table: "Friend",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Player_FriendPlayerId",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Player_PlayerId",
                table: "Friend");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Friend",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "FriendPlayerId",
                table: "Friend",
                newName: "FriendUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_PlayerId",
                table: "Friend",
                newName: "IX_Friend_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_FriendPlayerId",
                table: "Friend",
                newName: "IX_Friend_FriendUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_User_FriendUserId",
                table: "Friend",
                column: "FriendUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_User_UserId",
                table: "Friend",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
