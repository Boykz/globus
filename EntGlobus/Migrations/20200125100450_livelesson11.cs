using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntGlobus.Migrations
{
    public partial class livelesson11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "liveLessons");

            migrationBuilder.AddColumn<bool>(
                name: "OpenClose",
                table: "liveLessons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenClose",
                table: "liveLessons");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "liveLessons",
                nullable: true);
        }
    }
}
