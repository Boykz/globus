using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class migdeletebookvideoaccec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QrVideos_QrBooks_QrBookId",
                table: "QrVideos");

            migrationBuilder.DropIndex(
                name: "IX_QrVideos_QrBookId",
                table: "QrVideos");

            migrationBuilder.DropColumn(
                name: "Nuska",
                table: "QrVideos");

            migrationBuilder.DropColumn(
                name: "QrBookId",
                table: "QrVideos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Nuska",
                table: "QrVideos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "QrBookId",
                table: "QrVideos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_QrVideos_QrBookId",
                table: "QrVideos",
                column: "QrBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_QrVideos_QrBooks_QrBookId",
                table: "QrVideos",
                column: "QrBookId",
                principalTable: "QrBooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
