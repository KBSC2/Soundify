using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class PathToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathToImage",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathToImage",
                table: "Songs");
        }
    }
}
