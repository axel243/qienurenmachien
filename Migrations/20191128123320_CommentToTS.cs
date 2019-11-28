using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMachien.Migrations
{
    public partial class CommentToTS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "TimeSheets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "TimeSheets");
        }
    }
}
