using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "VisitorHelpOrder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "VisitorHelpOrder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "LotOrder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "LotOrder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_WorldObject_ClassName",
                table: "WorldObject",
                column: "ClassName");

            migrationBuilder.CreateIndex(
                name: "IX_WorldObject_WorldFlatId",
                table: "WorldObject",
                column: "WorldFlatId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorHelpOrder_OrderState_TransmissionStatus",
                table: "VisitorHelpOrder",
                columns: new[] { "OrderState", "TransmissionStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_VisitorHelpOrder_RecipientId",
                table: "VisitorHelpOrder",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorHelpOrder_SenderId",
                table: "VisitorHelpOrder",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserId",
                table: "User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quest_Name",
                table: "Quest",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Quest_QuestType",
                table: "Quest",
                column: "QuestType");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Snuid",
                table: "Player",
                column: "Snuid");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Uid",
                table: "Player",
                column: "Uid");

            migrationBuilder.CreateIndex(
                name: "IX_LotOrder_OrderState_TransmissionStatus",
                table: "LotOrder",
                columns: new[] { "OrderState", "TransmissionStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_LotOrder_RecipientId",
                table: "LotOrder",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_LotOrder_SenderId",
                table: "LotOrder",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_Status",
                table: "Friend",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorldObject_ClassName",
                table: "WorldObject");

            migrationBuilder.DropIndex(
                name: "IX_WorldObject_WorldFlatId",
                table: "WorldObject");

            migrationBuilder.DropIndex(
                name: "IX_VisitorHelpOrder_OrderState_TransmissionStatus",
                table: "VisitorHelpOrder");

            migrationBuilder.DropIndex(
                name: "IX_VisitorHelpOrder_RecipientId",
                table: "VisitorHelpOrder");

            migrationBuilder.DropIndex(
                name: "IX_VisitorHelpOrder_SenderId",
                table: "VisitorHelpOrder");

            migrationBuilder.DropIndex(
                name: "IX_User_UserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Quest_Name",
                table: "Quest");

            migrationBuilder.DropIndex(
                name: "IX_Quest_QuestType",
                table: "Quest");

            migrationBuilder.DropIndex(
                name: "IX_Player_Snuid",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_Uid",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_LotOrder_OrderState_TransmissionStatus",
                table: "LotOrder");

            migrationBuilder.DropIndex(
                name: "IX_LotOrder_RecipientId",
                table: "LotOrder");

            migrationBuilder.DropIndex(
                name: "IX_LotOrder_SenderId",
                table: "LotOrder");

            migrationBuilder.DropIndex(
                name: "IX_Friend_Status",
                table: "Friend");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "VisitorHelpOrder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "VisitorHelpOrder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "LotOrder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "LotOrder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
