using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorldObjectSupplyResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorldObjectSupplyResource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Position = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    WorldObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldObjectSupplyResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorldObjectSupplyResource_WorldObject_WorldObjectId",
                        column: x => x.WorldObjectId,
                        principalTable: "WorldObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorldObjectSupplyResource_WorldObjectId",
                table: "WorldObjectSupplyResource",
                column: "WorldObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorldObjectSupplyResource");
        }
    }
}
