using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations;

/// <inheritdoc />
public partial class _20261121114741_AddViralAck : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "AckSeen",
            table: "SocialPostHelper",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "TargetItemName",
            table: "SocialPost",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "TargetObjectId",
            table: "SocialPost",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "TargetObjectName",
            table: "SocialPost",
            type: "nvarchar(128)",
            maxLength: 128,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "TargetWorld",
            table: "SocialPost",
            type: "int",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AckSeen",
            table: "SocialPostHelper");

        migrationBuilder.DropColumn(
            name: "TargetItemName",
            table: "SocialPost");

        migrationBuilder.DropColumn(
            name: "TargetObjectId",
            table: "SocialPost");

        migrationBuilder.DropColumn(
            name: "TargetObjectName",
            table: "SocialPost");

        migrationBuilder.DropColumn(
            name: "TargetWorld",
            table: "SocialPost");
    }
}
