using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerWorldsCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "World",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorldCreated",
                table: "World",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE w
                SET w.PlayerId = p.Id
                FROM World w
                INNER JOIN Player p ON p.WorldId = w.Id
            """);

            migrationBuilder.DropForeignKey(
                name: "FK_Player_World_WorldId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_WorldId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "WorldId",
                table: "Player");

            migrationBuilder.CreateIndex(
                name: "IX_World_PlayerId",
                table: "World",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_World_Player_PlayerId",
                table: "World",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorldId",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE p
                SET p.WorldId = w.Id
                FROM Player p
                INNER JOIN World w ON w.PlayerId = p.Id AND w.Type = 0
            """);

            migrationBuilder.DropForeignKey(
                name: "FK_World_Player_PlayerId",
                table: "World");

            migrationBuilder.DropIndex(
                name: "IX_World_PlayerId",
                table: "World");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "World");

            migrationBuilder.DropColumn(
                name: "WorldCreated",
                table: "World");

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
    }
}
