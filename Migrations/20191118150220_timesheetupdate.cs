using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMachien.Migrations
{
    public partial class timesheetupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TimeSheets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TimeSheets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
