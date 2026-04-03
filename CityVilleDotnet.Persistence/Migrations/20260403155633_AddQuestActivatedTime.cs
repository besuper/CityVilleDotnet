using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestActivatedTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Quest",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Quest");
        }
    }
}
