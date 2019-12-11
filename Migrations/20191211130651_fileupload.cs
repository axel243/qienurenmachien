using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMachien.Migrations
{
    public partial class fileupload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_WerkgeverID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WerkgeverID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "WerkgeverID",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FileUploads",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(nullable: true),
                    SheetID = table.Column<int>(nullable: false),
                    FilePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploads", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_FileUploads_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileUploads_TimeSheets_SheetID",
                        column: x => x.SheetID,
                        principalTable: "TimeSheets",
                        principalColumn: "SheetID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_Id",
                table: "FileUploads",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_SheetID",
                table: "FileUploads",
                column: "SheetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileUploads");

            migrationBuilder.AlterColumn<string>(
                name: "WerkgeverID",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WerkgeverID",
                table: "AspNetUsers",
                column: "WerkgeverID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_WerkgeverID",
                table: "AspNetUsers",
                column: "WerkgeverID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
