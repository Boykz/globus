using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class qrmig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QrUserIdentities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QrUserIdentities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QrBookId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrUserIdentities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrUserIdentities_QrBooks_QrBookId",
                        column: x => x.QrBookId,
                        principalTable: "QrBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QrUserIdentities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QrUserIdentities_QrBookId",
                table: "QrUserIdentities",
                column: "QrBookId");

            migrationBuilder.CreateIndex(
                name: "IX_QrUserIdentities_UserId",
                table: "QrUserIdentities",
                column: "UserId");
        }
    }
}
