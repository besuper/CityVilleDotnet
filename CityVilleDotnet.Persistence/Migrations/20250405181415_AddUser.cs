using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commodities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Storage_Goods = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commodities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "World",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SizeX = table.Column<int>(type: "int", nullable: false),
                    SizeY = table.Column<int>(type: "int", nullable: false),
                    CitySim_Population = table.Column<int>(type: "int", nullable: true),
                    CitySim_PopulationCap = table.Column<int>(type: "int", nullable: true),
                    CitySim_PotentialPopulation = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_World", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Uid = table.Column<int>(type: "int", nullable: false),
                    LastTrackingTimestamp = table.Column<int>(type: "int", nullable: false),
                    Options_SfxDisabled = table.Column<bool>(type: "bit", nullable: true),
                    Options_MusicDisabled = table.Column<bool>(type: "bit", nullable: true),
                    CommoditiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Gold = table.Column<int>(type: "int", nullable: false),
                    Cash = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Xp = table.Column<int>(type: "int", nullable: false),
                    Energy = table.Column<int>(type: "int", nullable: false),
                    EnergyMax = table.Column<int>(type: "int", nullable: false),
                    ExpansionsPurchased = table.Column<int>(type: "int", nullable: false),
                    RollCounter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Commodities_CommoditiesId",
                        column: x => x.CommoditiesId,
                        principalTable: "Commodities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Player_Inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MapRect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    WorldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapRect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapRect_World_WorldId",
                        column: x => x.WorldId,
                        principalTable: "World",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorldObject",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    TempId = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direction = table.Column<int>(type: "int", nullable: false),
                    Position_X = table.Column<int>(type: "int", nullable: true),
                    Position_Y = table.Column<int>(type: "int", nullable: true),
                    Position_Z = table.Column<int>(type: "int", nullable: true),
                    WorldFlatId = table.Column<int>(type: "int", nullable: false),
                    WorldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldObject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorldObject_World_WorldId",
                        column: x => x.WorldId,
                        principalTable: "World",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    FirstDay = table.Column<bool>(type: "bit", nullable: false),
                    CreationTimestamp = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInfo_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserInfo_World_WorldId",
                        column: x => x.WorldId,
                        principalTable: "World",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_UserInfo_UserInfoId",
                        column: x => x.UserInfoId,
                        principalTable: "UserInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapRect_WorldId",
                table: "MapRect",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_CommoditiesId",
                table: "Player",
                column: "CommoditiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_InventoryId",
                table: "Player",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AppUserId",
                table: "User",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserInfoId",
                table: "User",
                column: "UserInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_PlayerId",
                table: "UserInfo",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_WorldId",
                table: "UserInfo",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_WorldObject_WorldId",
                table: "WorldObject",
                column: "WorldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapRect");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "WorldObject");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "World");

            migrationBuilder.DropTable(
                name: "Commodities");

            migrationBuilder.DropTable(
                name: "Inventory");
        }
    }
}
