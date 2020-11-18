using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class PlaylistUserColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Playlists",
                type: "int",
                nullable: false,
                defaultValue: true);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Playlists");
        }
    }
}
