using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class migaddvideoconnectnuska : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QrNuskaId",
                table: "QrVideos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QrVideos_QrNuskaId",
                table: "QrVideos",
                column: "QrNuskaId");

            migrationBuilder.AddForeignKey(
                name: "FK_QrVideos_QrNuskas_QrNuskaId",
                table: "QrVideos",
                column: "QrNuskaId",
                principalTable: "QrNuskas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QrVideos_QrNuskas_QrNuskaId",
                table: "QrVideos");

            migrationBuilder.DropIndex(
                name: "IX_QrVideos_QrNuskaId",
                table: "QrVideos");

            migrationBuilder.DropColumn(
                name: "QrNuskaId",
                table: "QrVideos");
        }
    }
}
