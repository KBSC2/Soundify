using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class SpelenMetForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserID",
                table: "Playlists",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_UserID",
                table: "Artists",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Artists_Users_UserID",
                table: "Artists",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Users_UserID",
                table: "Playlists",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artists_Users_UserID",
                table: "Artists");

            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Users_UserID",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_UserID",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_Artists_UserID",
                table: "Artists");
        }
    }
}
