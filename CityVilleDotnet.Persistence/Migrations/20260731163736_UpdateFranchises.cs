using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFranchises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM FranchiseLocation WHERE FranchiseId IS NULL");
            migrationBuilder.Sql("DELETE FROM Franchise WHERE PlayerId IS NULL");
            
            migrationBuilder.DropForeignKey(
                name: "FK_Franchise_Player_PlayerId",
                table: "Franchise");

            migrationBuilder.DropForeignKey(
                name: "FK_FranchiseLocation_Franchise_FranchiseId",
                table: "FranchiseLocation");

            migrationBuilder.AlterColumn<string>(
                name: "Uid",
                table: "FranchiseLocation",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ObjectId",
                table: "FranchiseLocation",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FranchiseName",
                table: "FranchiseLocation",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "FranchiseId",
                table: "FranchiseLocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId",
                table: "Franchise",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Franchise_Player_PlayerId",
                table: "Franchise",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FranchiseLocation_Franchise_FranchiseId",
                table: "FranchiseLocation",
                column: "FranchiseId",
                principalTable: "Franchise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Franchise_Player_PlayerId",
                table: "Franchise");

            migrationBuilder.DropForeignKey(
                name: "FK_FranchiseLocation_Franchise_FranchiseId",
                table: "FranchiseLocation");

            migrationBuilder.AlterColumn<string>(
                name: "Uid",
                table: "FranchiseLocation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "ObjectId",
                table: "FranchiseLocation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "FranchiseName",
                table: "FranchiseLocation",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<int>(
                name: "FranchiseId",
                table: "FranchiseLocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlayerId",
                table: "Franchise",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Franchise_Player_PlayerId",
                table: "Franchise",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FranchiseLocation_Franchise_FranchiseId",
                table: "FranchiseLocation",
                column: "FranchiseId",
                principalTable: "Franchise",
                principalColumn: "Id");
        }
    }
}
