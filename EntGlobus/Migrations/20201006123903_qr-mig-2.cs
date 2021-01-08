using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class qrmig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QrCode",
                table: "QrVideos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCode",
                table: "QrVideos");
        }
    }
}
