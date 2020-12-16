using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class AlteredAlbumTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumArtistSong_Album_AlbumId",
                table: "AlbumArtistSong");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumArtistSong_Artists_ArtistId",
                table: "AlbumArtistSong");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumArtistSong_Songs_SongId",
                table: "AlbumArtistSong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlbumArtistSong",
                table: "AlbumArtistSong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Album",
                table: "Album");

            migrationBuilder.RenameTable(
                name: "AlbumArtistSong",
                newName: "AlbumArtistSongs");

            migrationBuilder.RenameTable(
                name: "Album",
                newName: "Albums");

            migrationBuilder.RenameIndex(
                name: "IX_AlbumArtistSong_SongId",
                table: "AlbumArtistSongs",
                newName: "IX_AlbumArtistSongs_SongId");

            migrationBuilder.RenameIndex(
                name: "IX_AlbumArtistSong_ArtistId",
                table: "AlbumArtistSongs",
                newName: "IX_AlbumArtistSongs_ArtistId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlbumArtistSongs",
                table: "AlbumArtistSongs",
                columns: new[] { "AlbumId", "ArtistId", "SongId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Albums",
                table: "Albums",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumArtistSongs_Albums_AlbumId",
                table: "AlbumArtistSongs",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumArtistSongs_Artists_ArtistId",
                table: "AlbumArtistSongs",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumArtistSongs_Songs_SongId",
                table: "AlbumArtistSongs",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumArtistSongs_Albums_AlbumId",
                table: "AlbumArtistSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumArtistSongs_Artists_ArtistId",
                table: "AlbumArtistSongs");

            migrationBuilder.DropForeignKey(
                name: "FK_AlbumArtistSongs_Songs_SongId",
                table: "AlbumArtistSongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Albums",
                table: "Albums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlbumArtistSongs",
                table: "AlbumArtistSongs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Albums");

            migrationBuilder.RenameTable(
                name: "Albums",
                newName: "Album");

            migrationBuilder.RenameTable(
                name: "AlbumArtistSongs",
                newName: "AlbumArtistSong");

            migrationBuilder.RenameIndex(
                name: "IX_AlbumArtistSongs_SongId",
                table: "AlbumArtistSong",
                newName: "IX_AlbumArtistSong_SongId");

            migrationBuilder.RenameIndex(
                name: "IX_AlbumArtistSongs_ArtistId",
                table: "AlbumArtistSong",
                newName: "IX_AlbumArtistSong_ArtistId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Album",
                table: "Album",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlbumArtistSong",
                table: "AlbumArtistSong",
                columns: new[] { "AlbumId", "ArtistId", "SongId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumArtistSong_Album_AlbumId",
                table: "AlbumArtistSong",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumArtistSong_Artists_ArtistId",
                table: "AlbumArtistSong",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumArtistSong_Songs_SongId",
                table: "AlbumArtistSong",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
