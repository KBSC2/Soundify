using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Model.Enums;

namespace Model.Migrations
{
    public partial class AddPermissionsToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var perm in Enum.GetValues(typeof(Permissions)))
            {
                migrationBuilder.InsertData(
                    table: "Permissions",
                    columns: new[] { "Name", "HasValue" },
                    values: new object[]
                    {
                        perm.ToString(), perm.ToString().Contains("Limit")
                    });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var perm in Enum.GetValues(typeof(Permissions)))
            {
                migrationBuilder.DeleteData(
                    table: "Permissions",
                    keyColumn: "ID",
                    keyValues: new object[] {(int) perm});
            }
        }
    }
}
