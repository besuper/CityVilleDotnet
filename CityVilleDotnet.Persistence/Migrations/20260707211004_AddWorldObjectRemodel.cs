using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorldObjectRemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemodelBuilds",
                table: "WorldObject",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemodelItemName",
                table: "WorldObject",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemodelBuilds",
                table: "WorldObject");

            migrationBuilder.DropColumn(
                name: "RemodelItemName",
                table: "WorldObject");
        }
    }
}
