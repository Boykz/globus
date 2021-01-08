using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class qrmig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QrVideos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QrVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QrBookId = table.Column<Guid>(nullable: false),
                    QrCode = table.Column<int>(nullable: false),
                    Stats = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VideoUrl = table.Column<string>(nullable: true)
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
                name: "IX_QrVideos_QrBookId",
                table: "QrVideos",
                column: "QrBookId");
        }
    }
}
