using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNextBuildingId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextBuildingId",
                table: "World",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE w
                SET w.NextBuildingId = ISNULL(
                    (SELECT MAX(wo.WorldFlatId) + 1 FROM WorldObject wo WHERE wo.WorldId = w.Id), 1)
                FROM World w
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextBuildingId",
                table: "World");
        }
    }
}
