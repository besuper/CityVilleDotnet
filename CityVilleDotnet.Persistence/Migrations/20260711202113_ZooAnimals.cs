using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ZooAnimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorldObjectSlot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlotIndex = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    WorldObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldObjectSlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorldObjectSlot_WorldObject_WorldObjectId",
                        column: x => x.WorldObjectId,
                        principalTable: "WorldObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorldObjectStorageItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    WorldObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldObjectStorageItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorldObjectStorageItem_WorldObject_WorldObjectId",
                        column: x => x.WorldObjectId,
                        principalTable: "WorldObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorldObjectSlot_WorldObjectId",
                table: "WorldObjectSlot",
                column: "WorldObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorldObjectStorageItem_WorldObjectId",
                table: "WorldObjectStorageItem",
                column: "WorldObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorldObjectSlot");

            migrationBuilder.DropTable(
                name: "WorldObjectStorageItem");
        }
    }
}
