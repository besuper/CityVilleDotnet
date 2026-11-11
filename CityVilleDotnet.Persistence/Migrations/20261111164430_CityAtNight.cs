using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations;

/// <inheritdoc />
public partial class _20261111164430_CityAtNight : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "CityAtNightEarlyUnlocked",
            table: "Player",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "CityLightCount",
            table: "Player",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "LightExpansionsPurchased",
            table: "Player",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "LightLevel",
            table: "Player",
            type: "int",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.AddColumn<bool>(
            name: "NightModeAvailable",
            table: "Player",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.Sql("UPDATE [Player] SET [CityAtNightEarlyUnlocked] = 1, [NightModeAvailable] = 1");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CityAtNightEarlyUnlocked",
            table: "Player");

        migrationBuilder.DropColumn(
            name: "CityLightCount",
            table: "Player");

        migrationBuilder.DropColumn(
            name: "LightExpansionsPurchased",
            table: "Player");

        migrationBuilder.DropColumn(
            name: "LightLevel",
            table: "Player");

        migrationBuilder.DropColumn(
            name: "NightModeAvailable",
            table: "Player");
    }
}
