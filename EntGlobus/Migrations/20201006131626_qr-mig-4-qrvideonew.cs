using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class qrmig4qrvideonew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QrVideos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    VideoUrl = table.Column<string>(nullable: true),
                    Stats = table.Column<bool>(nullable: false),
                    QrCode = table.Column<int>(nullable: false),
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
                name: "IX_QrVideos_QrBookId",
                table: "QrVideos",
                column: "QrBookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QrVideos");
        }
    }
}
