using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class addedShopItemTabbles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ShopItemPersmissions",
                columns: table => new
                {
                    ShopItemID = table.Column<int>(type: "int", nullable: false),
                    PermissionID = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemPersmissions", x => new { x.ShopItemID, x.PermissionID });
                    table.ForeignKey(
                        name: "FK_ShopItemPersmissions_Permissions_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopItemPersmissions_ShopItems_ShopItemID",
                        column: x => x.ShopItemID,
                        principalTable: "ShopItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserShopItems",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ShopItemID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShopItems", x => new { x.UserID, x.ShopItemID });
                    table.ForeignKey(
                        name: "FK_UserShopItems_ShopItems_ShopItemID",
                        column: x => x.ShopItemID,
                        principalTable: "ShopItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserShopItems_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemPersmissions_PermissionID",
                table: "ShopItemPersmissions",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_UserShopItems_ShopItemID",
                table: "UserShopItems",
                column: "ShopItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopItemPersmissions");

            migrationBuilder.DropTable(
                name: "UserShopItems");

            migrationBuilder.DropTable(
                name: "ShopItems");
        }
    }
}
