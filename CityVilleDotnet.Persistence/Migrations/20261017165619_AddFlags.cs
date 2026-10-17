using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flag_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flag_PlayerId",
                table: "Flag",
                column: "PlayerId");

            migrationBuilder.Sql("""
                INSERT INTO [Flag] ([Name], [Value], [LastModified], [PlayerId])
                SELECT 'completed_tutorial', 1, SYSDATETIMEOFFSET(), [Id]
                FROM [Player]
                WHERE [IsNew] = 0
                """);

            migrationBuilder.Sql("""
                INSERT INTO [Flag] ([Name], [Value], [LastModified], [PlayerId])
                SELECT DISTINCT 'completed_bridge', 1, SYSDATETIMEOFFSET(), w.[PlayerId]
                FROM [World] w
                JOIN [WorldObject] o ON o.[WorldId] = w.[Id]
                WHERE w.[Type] = 0 AND o.[ClassName] = 19 AND w.[PlayerId] IS NOT NULL
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flag");
        }
    }
}
