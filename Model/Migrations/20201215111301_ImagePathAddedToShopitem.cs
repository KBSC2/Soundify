using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class ImagePathAddedToShopitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ShopItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ShopItems");
        }
    }
}
