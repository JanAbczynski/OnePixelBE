using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class UserPermissionfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionId",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_RawPermissionId",
                table: "UserPermissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_RawPermissionId",
                table: "UserPermissions",
                column: "RawPermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionId",
                table: "UserPermissions",
                column: "RawPermissionId",
                principalTable: "AvaliblePermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
