using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMachien.Migrations
{
    public partial class DateTimetimesheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "theDate",
                table: "TimeSheets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "theDate",
                table: "TimeSheets");
        }
    }
}
