using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class qrmig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QrBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BookName = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QrUserIdentities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    QrBookId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrUserIdentities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrUserIdentities_QrBooks_QrBookId",
                        column: x => x.QrBookId,
                        principalTable: "QrBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QrUserIdentities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QrVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VideoUrl = table.Column<string>(nullable: true),
                    QrBookId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrVideos_QrBooks_QrBookId",
                        column: x => x.QrBookId,
                        principalTable: "QrBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QrUserIdentities_QrBookId",
                table: "QrUserIdentities",
                column: "QrBookId");

            migrationBuilder.CreateIndex(
                name: "IX_QrUserIdentities_UserId",
                table: "QrUserIdentities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QrVideos_QrBookId",
                table: "QrVideos",
                column: "QrBookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QrUserIdentities");

            migrationBuilder.DropTable(
                name: "QrVideos");

            migrationBuilder.DropTable(
                name: "QrBooks");
        }
    }
}
