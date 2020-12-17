using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class AddForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumArtistSongs");

            migrationBuilder.RenameColumn(
                name: "Artist",
                table: "Songs",
                newName: "ArtistID");

            migrationBuilder.AddColumn<int>(
                name: "AlbumID",
                table: "Songs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArtistID",
                table: "Albums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_AlbumID",
                table: "Songs",
                column: "AlbumID");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ArtistID",
                table: "Songs",
                column: "ArtistID");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserID",
                table: "Playlists",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_UserID",
                table: "Artists",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistID",
                table: "Albums",
                column: "ArtistID");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Artists_ArtistID",
                table: "Songs",
                column: "ArtistID",
                principalTable: "Artists",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Artists_ArtistID",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Users_UserID",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Users_UserID",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Albums_AlbumID",
                table: "Songs");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Artists_ArtistID",
                table: "Songs");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ShopItemPersmissions");

            migrationBuilder.DropIndex(
                name: "IX_Songs_AlbumID",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_ArtistID",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_UserID",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_Artists_UserID",
                table: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_Albums_ArtistID",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "AlbumID",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "ArtistID",
                table: "Albums");

            migrationBuilder.RenameColumn(
                name: "ArtistID",
                table: "Songs",
                newName: "Artist");

            migrationBuilder.AddColumn<string>(
                name: "AlbumName",
                table: "Albums",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AlbumArtistSongs",
                columns: table => new
                {
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumArtistSongs", x => new { x.AlbumId, x.ArtistId, x.SongId });
                    table.ForeignKey(
                        name: "FK_AlbumArtistSongs_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumArtistSongs_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumArtistSongs_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumArtistSongs_ArtistId",
                table: "AlbumArtistSongs",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumArtistSongs_SongId",
                table: "AlbumArtistSongs",
                column: "SongId");
        }
    }
}
