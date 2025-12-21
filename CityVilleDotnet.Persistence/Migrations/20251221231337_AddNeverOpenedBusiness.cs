using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNeverOpenedBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeverOpened",
                table: "WorldObject",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeverOpened",
                table: "WorldObject");
        }
    }
}
