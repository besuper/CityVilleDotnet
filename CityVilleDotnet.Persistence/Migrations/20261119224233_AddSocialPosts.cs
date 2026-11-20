using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityVilleDotnet.Persistence.Migrations;

/// <inheritdoc />
public partial class _20261119224233_AddSocialPosts : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "SocialNotificationsSeenAt",
            table: "Player",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.CreateTable(
            name: "SocialPost",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TargetPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Type = table.Column<int>(type: "int", nullable: false),
                Key = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UserMessage = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                MaxHelpers = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SocialPost", x => x.Id);
                table.ForeignKey(
                    name: "FK_SocialPost_Player_PlayerId",
                    column: x => x.PlayerId,
                    principalTable: "Player",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SocialPost_Player_TargetPlayerId",
                    column: x => x.TargetPlayerId,
                    principalTable: "Player",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "SocialPostHelper",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                SocialPostId = table.Column<int>(type: "int", nullable: false),
                HelperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HelpedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SocialPostHelper", x => x.Id);
                table.ForeignKey(
                    name: "FK_SocialPostHelper_Player_HelperId",
                    column: x => x.HelperId,
                    principalTable: "Player",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_SocialPostHelper_SocialPost_SocialPostId",
                    column: x => x.SocialPostId,
                    principalTable: "SocialPost",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SocialPost_ExpiresAt",
            table: "SocialPost",
            column: "ExpiresAt");

        migrationBuilder.CreateIndex(
            name: "IX_SocialPost_PlayerId_CreatedAt",
            table: "SocialPost",
            columns: new[] { "PlayerId", "CreatedAt" });

        migrationBuilder.CreateIndex(
            name: "IX_SocialPost_TargetPlayerId",
            table: "SocialPost",
            column: "TargetPlayerId");

        migrationBuilder.CreateIndex(
            name: "IX_SocialPostHelper_HelpedAt",
            table: "SocialPostHelper",
            column: "HelpedAt");

        migrationBuilder.CreateIndex(
            name: "IX_SocialPostHelper_HelperId",
            table: "SocialPostHelper",
            column: "HelperId");

        migrationBuilder.CreateIndex(
            name: "IX_SocialPostHelper_SocialPostId_HelperId",
            table: "SocialPostHelper",
            columns: new[] { "SocialPostId", "HelperId" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SocialPostHelper");

        migrationBuilder.DropTable(
            name: "SocialPost");

        migrationBuilder.DropColumn(
            name: "SocialNotificationsSeenAt",
            table: "Player");
    }
}
