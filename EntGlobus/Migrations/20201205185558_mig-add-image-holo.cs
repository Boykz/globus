using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class migaddimageholo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "QrBooks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "QrBooks");
        }
    }
}
