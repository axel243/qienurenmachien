using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMachien.Migrations
{
    public partial class IdentityFK2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeSheet",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SheetID = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Project = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    ProjectHours = table.Column<double>(nullable: false),
                    Overwork = table.Column<double>(nullable: false),
                    Sick = table.Column<double>(nullable: false),
                    Absence = table.Column<double>(nullable: false),
                    Training = table.Column<double>(nullable: false),
                    Other = table.Column<double>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSheet_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSheet");
        }
    }
}
