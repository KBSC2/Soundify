using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class ChangedColumnToNullableint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Songs_SongID",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "SongID",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Songs_SongID",
                table: "Requests",
                column: "SongID",
                principalTable: "Songs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Songs_SongID",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "SongID",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Songs_SongID",
                table: "Requests",
                column: "SongID",
                principalTable: "Songs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
