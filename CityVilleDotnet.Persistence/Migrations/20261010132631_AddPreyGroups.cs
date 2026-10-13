using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPreyGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreyGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    NumPreysCaptured = table.Column<int>(type: "int", nullable: false),
                    GateUnlockLevel = table.Column<int>(type: "int", nullable: false),
                    LastQueueRefill = table.Column<long>(type: "bigint", nullable: false),
                    Preys = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreyGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreyGroup_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreyCapture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreyId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    PreyGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreyCapture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreyCapture_PreyGroup_PreyGroupId",
                        column: x => x.PreyGroupId,
                        principalTable: "PreyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreyHunter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    PreyGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreyHunter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreyHunter_PreyGroup_PreyGroupId",
                        column: x => x.PreyGroupId,
                        principalTable: "PreyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreyCapture_PreyGroupId_PreyId",
                table: "PreyCapture",
                columns: new[] { "PreyGroupId", "PreyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreyGroup_PlayerId_GroupId",
                table: "PreyGroup",
                columns: new[] { "PlayerId", "GroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreyHunter_PreyGroupId_Position",
                table: "PreyHunter",
                columns: new[] { "PreyGroupId", "Position" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreyCapture");

            migrationBuilder.DropTable(
                name: "PreyHunter");

            migrationBuilder.DropTable(
                name: "PreyGroup");
        }
    }
}
