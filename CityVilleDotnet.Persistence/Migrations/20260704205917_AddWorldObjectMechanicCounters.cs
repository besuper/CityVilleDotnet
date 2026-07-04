using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorldObjectMechanicCounters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorldObjectMechanicCounter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MechanicType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    WorldObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldObjectMechanicCounter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorldObjectMechanicCounter_WorldObject_WorldObjectId",
                        column: x => x.WorldObjectId,
                        principalTable: "WorldObject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorldObjectMechanicCounter_WorldObjectId",
                table: "WorldObjectMechanicCounter",
                column: "WorldObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorldObjectMechanicCounter");
        }
    }
}
