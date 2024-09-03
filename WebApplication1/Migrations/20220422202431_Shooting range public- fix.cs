using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class Shootingrangepublicfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "ShootingRanges");

            migrationBuilder.RenameColumn(
                name: "isPublic",
                table: "ShootingRanges",
                newName: "IsPublic");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "ShootingRanges",
                newName: "isPublic");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "ShootingRanges",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
