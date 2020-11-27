using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class AddedFieldsForSongInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProducedBy",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WrittenBy",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "ProducedBy",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "WrittenBy",
                table: "Songs");
        }
    }
}
