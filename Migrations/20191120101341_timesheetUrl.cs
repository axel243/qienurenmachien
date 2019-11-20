using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMachien.Migrations
{
    public partial class timesheetUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "TimeSheets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "TimeSheets");
        }
    }
}
