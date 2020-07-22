using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class nishfirst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NishCourses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NishCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NishPays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NishCourseId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    OpenDate = table.Column<DateTime>(nullable: false),
                    CloseDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NishPays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NishPays_NishCourses_NishCourseId",
                        column: x => x.NishCourseId,
                        principalTable: "NishCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NishPays_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NishPays_NishCourseId",
                table: "NishPays",
                column: "NishCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_NishPays_UserId",
                table: "NishPays",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NishPays");

            migrationBuilder.DropTable(
                name: "NishCourses");
        }
    }
}
