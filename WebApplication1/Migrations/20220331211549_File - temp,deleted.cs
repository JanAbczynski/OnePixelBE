using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class Filetempdeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "WebFile",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isTemp",
                table: "WebFile",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "WebFile");

            migrationBuilder.DropColumn(
                name: "isTemp",
                table: "WebFile");
        }
    }
}
