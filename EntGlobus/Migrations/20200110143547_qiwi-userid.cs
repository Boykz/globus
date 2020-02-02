using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class qiwiuserid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Qiwipays",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Qiwipays_UserId",
                table: "Qiwipays",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Qiwipays_AspNetUsers_UserId",
                table: "Qiwipays",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Qiwipays_AspNetUsers_UserId",
                table: "Qiwipays");

            migrationBuilder.DropIndex(
                name: "IX_Qiwipays_UserId",
                table: "Qiwipays");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Qiwipays");
        }
    }
}
