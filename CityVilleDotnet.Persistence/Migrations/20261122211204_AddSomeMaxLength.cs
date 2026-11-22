using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations;

/// <inheritdoc />
public partial class _20261122211204_AddSomeMaxLength : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "ItemOwner",
            table: "WorldObject",
            type: "nvarchar(32)",
            maxLength: 32,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Purchased",
            table: "Quest",
            type: "nvarchar(512)",
            maxLength: 512,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Progress",
            table: "Quest",
            type: "nvarchar(512)",
            maxLength: 512,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Preys",
            table: "PreyGroup",
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
            name: "ItemOwner",
            table: "WorldObject",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(32)",
            oldMaxLength: 32,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Purchased",
            table: "Quest",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(512)",
            oldMaxLength: 512);

        migrationBuilder.AlterColumn<string>(
            name: "Progress",
            table: "Quest",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(512)",
            oldMaxLength: 512);

        migrationBuilder.AlterColumn<string>(
            name: "Preys",
            table: "PreyGroup",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(64)",
            oldMaxLength: 64);
    }
}
