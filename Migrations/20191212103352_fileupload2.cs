using Microsoft.EntityFrameworkCore.Migrations;

namespace QienUrenMachien.Migrations
{
    public partial class fileupload2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_TimeSheets_SheetID",
                table: "FileUploads");

            migrationBuilder.AlterColumn<int>(
                name: "SheetID",
                table: "FileUploads",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_TimeSheets_SheetID",
                table: "FileUploads",
                column: "SheetID",
                principalTable: "TimeSheets",
                principalColumn: "SheetID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_TimeSheets_SheetID",
                table: "FileUploads");

            migrationBuilder.AlterColumn<int>(
                name: "SheetID",
                table: "FileUploads",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_TimeSheets_SheetID",
                table: "FileUploads",
                column: "SheetID",
                principalTable: "TimeSheets",
                principalColumn: "SheetID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
