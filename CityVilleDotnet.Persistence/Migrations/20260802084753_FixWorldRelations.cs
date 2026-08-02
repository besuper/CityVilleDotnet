using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixWorldRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM MapRect WHERE WorldId IS NULL");
            migrationBuilder.Sql("DELETE FROM IncentivizedExpansion WHERE WorldId IS NULL");
            
            migrationBuilder.DropForeignKey(
                name: "FK_IncentivizedExpansion_World_WorldId",
                table: "IncentivizedExpansion");

            migrationBuilder.DropForeignKey(
                name: "FK_MapRect_World_WorldId",
                table: "MapRect");

            migrationBuilder.AlterColumn<int>(
                name: "WorldId",
                table: "MapRect",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WorldId",
                table: "IncentivizedExpansion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IncentivizedExpansion_World_WorldId",
                table: "IncentivizedExpansion",
                column: "WorldId",
                principalTable: "World",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapRect_World_WorldId",
                table: "MapRect",
                column: "WorldId",
                principalTable: "World",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncentivizedExpansion_World_WorldId",
                table: "IncentivizedExpansion");

            migrationBuilder.DropForeignKey(
                name: "FK_MapRect_World_WorldId",
                table: "MapRect");

            migrationBuilder.AlterColumn<int>(
                name: "WorldId",
                table: "MapRect",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "WorldId",
                table: "IncentivizedExpansion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_IncentivizedExpansion_World_WorldId",
                table: "IncentivizedExpansion",
                column: "WorldId",
                principalTable: "World",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapRect_World_WorldId",
                table: "MapRect",
                column: "WorldId",
                principalTable: "World",
                principalColumn: "Id");
        }
    }
}
