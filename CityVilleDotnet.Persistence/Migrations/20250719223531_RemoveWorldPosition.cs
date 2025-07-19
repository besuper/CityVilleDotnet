using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWorldPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position_Z",
                table: "WorldObject",
                newName: "Z");

            migrationBuilder.RenameColumn(
                name: "Position_Y",
                table: "WorldObject",
                newName: "Y");

            migrationBuilder.RenameColumn(
                name: "Position_X",
                table: "WorldObject",
                newName: "X");

            migrationBuilder.AlterColumn<int>(
                name: "Y",
                table: "WorldObject",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "X",
                table: "WorldObject",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Z",
                table: "WorldObject",
                newName: "Position_Z");

            migrationBuilder.RenameColumn(
                name: "Y",
                table: "WorldObject",
                newName: "Position_Y");

            migrationBuilder.RenameColumn(
                name: "X",
                table: "WorldObject",
                newName: "Position_X");

            migrationBuilder.AlterColumn<int>(
                name: "Position_Y",
                table: "WorldObject",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Position_X",
                table: "WorldObject",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
