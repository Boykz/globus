using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class livelessons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthType",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "liveLessons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_liveLessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PodLiveLessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UrlVideo = table.Column<string>(nullable: true),
                    UrlPhoto = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: true),
                    TypeVideo = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    LiveLessonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PodLiveLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PodLiveLessons_liveLessons_LiveLessonId",
                        column: x => x.LiveLessonId,
                        principalTable: "liveLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PodLiveLessons_LiveLessonId",
                table: "PodLiveLessons",
                column: "LiveLessonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PodLiveLessons");

            migrationBuilder.DropTable(
                name: "liveLessons");

            migrationBuilder.DropColumn(
                name: "AuthType",
                table: "AspNetUsers");
        }
    }
}
