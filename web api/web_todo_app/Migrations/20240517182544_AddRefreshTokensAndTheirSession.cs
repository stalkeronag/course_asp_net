using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_todo_app.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokensAndTheirSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokenSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokenSessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokenSessionConnections",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RefreshTokenSessionId = table.Column<string>(type: "text", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenSessionConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokenSessionConnections_RefreshTokenSessions_Refresh~",
                        column: x => x.RefreshTokenSessionId,
                        principalTable: "RefreshTokenSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FingerPrints",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    Referer = table.Column<string>(type: "text", nullable: true),
                    Hash = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenSessionConnectionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerPrints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FingerPrints_RefreshTokenSessionConnections_RefreshTokenSes~",
                        column: x => x.RefreshTokenSessionConnectionId,
                        principalTable: "RefreshTokenSessionConnections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RefreshTokenSessionConnectionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_RefreshTokenSessionConnections_RefreshTokenSe~",
                        column: x => x.RefreshTokenSessionConnectionId,
                        principalTable: "RefreshTokenSessionConnections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FingerPrints_RefreshTokenSessionConnectionId",
                table: "FingerPrints",
                column: "RefreshTokenSessionConnectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_RefreshTokenSessionConnectionId",
                table: "RefreshTokens",
                column: "RefreshTokenSessionConnectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenSessionConnections_RefreshTokenSessionId",
                table: "RefreshTokenSessionConnections",
                column: "RefreshTokenSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenSessions_UserId",
                table: "RefreshTokenSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FingerPrints");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RefreshTokenSessionConnections");

            migrationBuilder.DropTable(
                name: "RefreshTokenSessions");
        }
    }
}
