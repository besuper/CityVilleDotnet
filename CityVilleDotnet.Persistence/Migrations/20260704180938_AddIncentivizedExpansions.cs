using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIncentivizedExpansions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncentivizedExpansion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpansionId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    X = table.Column<int>(type: "int", nullable: true),
                    Y = table.Column<int>(type: "int", nullable: true),
                    StartTimestamp = table.Column<long>(type: "bigint", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    FailureCount = table.Column<int>(type: "int", nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncentivizedExpansion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncentivizedExpansion_World_WorldId",
                        column: x => x.WorldId,
                        principalTable: "World",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncentivizedExpansion_WorldId",
                table: "IncentivizedExpansion",
                column: "WorldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncentivizedExpansion");
        }
    }
}
