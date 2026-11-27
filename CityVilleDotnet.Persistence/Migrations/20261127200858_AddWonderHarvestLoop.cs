using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations;

/// <inheritdoc />
public partial class _20261127200858_AddWonderHarvestLoop : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "AppliedCoupon",
            table: "WorldObject",
            type: "nvarchar(64)",
            maxLength: 64,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "HarvestLoopStartTime",
            table: "WorldObject",
            type: "datetime2",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AppliedCoupon",
            table: "WorldObject");

        migrationBuilder.DropColumn(
            name: "HarvestLoopStartTime",
            table: "WorldObject");
    }
}
