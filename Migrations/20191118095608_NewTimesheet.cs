using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMachien.Migrations
{
    public partial class NewTimesheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeSheets",
                columns: table => new
                {
                    SheetID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Project = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    ProjectHours = table.Column<double>(nullable: false),
                    Overwork = table.Column<double>(nullable: false),
                    Sick = table.Column<double>(nullable: false),
                    Absence = table.Column<double>(nullable: false),
                    Training = table.Column<double>(nullable: false),
                    Other = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: true),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheets", x => x.SheetID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSheets");
        }
    }
}
