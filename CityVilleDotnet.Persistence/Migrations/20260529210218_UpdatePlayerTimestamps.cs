using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlayerTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastTrackingTimestamp_New",
                table: "Player",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: DateTimeOffset.Now);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreationTimestamp_New",
                table: "Player",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: DateTimeOffset.Now);
            
            migrationBuilder.DropColumn(name: "LastTrackingTimestamp", table: "Player");
            migrationBuilder.DropColumn(name: "CreationTimestamp",     table: "Player");
            
            migrationBuilder.RenameColumn(
                name: "LastTrackingTimestamp_New",
                table: "Player",
                newName: "LastTrackingTimestamp");

            migrationBuilder.RenameColumn(
                name: "CreationTimestamp_New",
                table: "Player",
                newName: "CreationTimestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastTrackingTimestamp_Old",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreationTimestamp_Old",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.DropColumn(name: "LastTrackingTimestamp", table: "Player");
            migrationBuilder.DropColumn(name: "CreationTimestamp",     table: "Player");

            migrationBuilder.RenameColumn(
                name: "LastTrackingTimestamp_Old",
                table: "Player",
                newName: "LastTrackingTimestamp");

            migrationBuilder.RenameColumn(
                name: "CreationTimestamp_Old",
                table: "Player",
                newName: "CreationTimestamp");
        }
    }
}
