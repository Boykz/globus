using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class chatlivelessons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiveChats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    MessDate = table.Column<DateTime>(nullable: true),
                    PodLiveLessonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiveChats_PodLiveLessons_PodLiveLessonId",
                        column: x => x.PodLiveLessonId,
                        principalTable: "PodLiveLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveChats_PodLiveLessonId",
                table: "LiveChats",
                column: "PodLiveLessonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiveChats");
        }
    }
}
