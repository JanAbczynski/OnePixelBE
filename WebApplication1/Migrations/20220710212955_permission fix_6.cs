using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class permissionfix_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionID",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_RawPermissionID",
                table: "UserPermissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_RawPermissionID",
                table: "UserPermissions",
                column: "RawPermissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionID",
                table: "UserPermissions",
                column: "RawPermissionID",
                principalTable: "AvaliblePermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
