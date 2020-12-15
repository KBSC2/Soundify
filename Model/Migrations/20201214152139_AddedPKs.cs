using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class AddedPKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserShopItems",
                table: "UserShopItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopItemPersmissions",
                table: "ShopItemPersmissions");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "UserShopItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ShopItemPersmissions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserShopItems",
                table: "UserShopItems",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopItemPersmissions",
                table: "ShopItemPersmissions",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_UserShopItems_UserID",
                table: "UserShopItems",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemPersmissions_ShopItemID",
                table: "ShopItemPersmissions",
                column: "ShopItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserShopItems",
                table: "UserShopItems");

            migrationBuilder.DropIndex(
                name: "IX_UserShopItems_UserID",
                table: "UserShopItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopItemPersmissions",
                table: "ShopItemPersmissions");

            migrationBuilder.DropIndex(
                name: "IX_ShopItemPersmissions_ShopItemID",
                table: "ShopItemPersmissions");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "UserShopItems");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ShopItemPersmissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserShopItems",
                table: "UserShopItems",
                columns: new[] { "UserID", "ShopItemID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopItemPersmissions",
                table: "ShopItemPersmissions",
                columns: new[] { "ShopItemID", "PermissionID" });
        }
    }
}
