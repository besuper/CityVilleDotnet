using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompleteQuest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complete",
                table: "Quest");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Quest",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Quest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<int>(
                name: "Complete",
                table: "Quest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
