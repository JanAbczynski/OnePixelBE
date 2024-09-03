using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class permissionfix_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionId",
                table: "UserPermissions");

            migrationBuilder.RenameColumn(
                name: "RawPermissionId",
                table: "UserPermissions",
                newName: "RawPermissionID");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermissions_RawPermissionId",
                table: "UserPermissions",
                newName: "IX_UserPermissions_RawPermissionID");

            migrationBuilder.AlterColumn<Guid>(
                name: "RawPermissionID",
                table: "UserPermissions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionID",
                table: "UserPermissions",
                column: "RawPermissionID",
                principalTable: "AvaliblePermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionID",
                table: "UserPermissions");

            migrationBuilder.RenameColumn(
                name: "RawPermissionID",
                table: "UserPermissions",
                newName: "RawPermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermissions_RawPermissionID",
                table: "UserPermissions",
                newName: "IX_UserPermissions_RawPermissionId");

            migrationBuilder.AlterColumn<Guid>(
                name: "RawPermissionId",
                table: "UserPermissions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionId",
                table: "UserPermissions",
                column: "RawPermissionId",
                principalTable: "AvaliblePermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
