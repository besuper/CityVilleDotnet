using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveWorldInPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_World_WorldId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_WorldId",
                table: "User");
            
            migrationBuilder.AddColumn<int>(
                name: "WorldId",
                table: "Player",
                type: "int",
                nullable: true);
            
            migrationBuilder.Sql("""
                UPDATE p 
                SET p.WorldId = u.WorldId 
                FROM Player p 
                INNER JOIN [User] u ON u.PlayerId = p.Id
            """);

            migrationBuilder.DropColumn(
                name: "WorldId",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Player_WorldId",
                table: "Player",
                column: "WorldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_World_WorldId",
                table: "Player",
                column: "WorldId",
                principalTable: "World",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_World_WorldId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_WorldId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "WorldId",
                table: "Player");

            migrationBuilder.AddColumn<int>(
                name: "WorldId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_WorldId",
                table: "User",
                column: "WorldId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_World_WorldId",
                table: "User",
                column: "WorldId",
                principalTable: "World",
                principalColumn: "Id");
        }
    }
}
