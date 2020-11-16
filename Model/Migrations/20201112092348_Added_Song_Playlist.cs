using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class Added_Song_Playlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table => { table.PrimaryKey("PK_Playlists", x => x.ID); });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Songs", x => x.ID); });

            migrationBuilder.CreateTable(
                name: "PlaylistSongs",
                columns: table => new
                {
                    SongID = table.Column<int>(type: "int", nullable: false),
                    PlaylistID = table.Column<int>(type: "int", nullable: false),
                    Added = table.Column<DateTime>(type: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSongs", x => new {x.PlaylistID, x.SongID});
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Playlists_PlaylistID",
                        column: x => x.PlaylistID,
                        principalTable: "Playlists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistSongs_Songs_SongID",
                        column: x => x.SongID,
                        principalTable: "Songs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongs_SongID",
                table: "PlaylistSongs",
                column: "SongID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistSongs");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Songs");
        }
    }
}