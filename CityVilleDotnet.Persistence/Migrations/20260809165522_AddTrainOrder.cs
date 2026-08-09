using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Operation = table.Column<int>(type: "int", nullable: false),
                    CommodityName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TimeSent = table.Column<long>(type: "bigint", nullable: false),
                    WorldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainOrder_World_WorldId",
                        column: x => x.WorldId,
                        principalTable: "World",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainOrderWorker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Zid = table.Column<int>(type: "int", nullable: false),
                    TrainOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainOrderWorker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainOrderWorker_TrainOrder_TrainOrderId",
                        column: x => x.TrainOrderId,
                        principalTable: "TrainOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainOrder_WorldId",
                table: "TrainOrder",
                column: "WorldId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainOrderWorker_TrainOrderId",
                table: "TrainOrderWorker",
                column: "TrainOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainOrderWorker");

            migrationBuilder.DropTable(
                name: "TrainOrder");
        }
    }
}
