using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class eAllCourse1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Decription",
                table: "AllCourses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url_Img",
                table: "AllCourses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Decription",
                table: "AllCourses");

            migrationBuilder.DropColumn(
                name: "Url_Img",
                table: "AllCourses");
        }
    }
}
