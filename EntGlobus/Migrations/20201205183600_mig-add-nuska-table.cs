using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class migaddnuskatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QrNuskas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NuskaNumber = table.Column<int>(nullable: false),
                    QrBookId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrNuskas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrNuskas_QrBooks_QrBookId",
                        column: x => x.QrBookId,
                        principalTable: "QrBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QrNuskas_QrBookId",
                table: "QrNuskas",
                column: "QrBookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QrNuskas");
        }
    }
}
