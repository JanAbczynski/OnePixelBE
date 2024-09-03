using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class permissionfix_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentsNumber",
                table: "UserPermissions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "UserPermissions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "RawPermissionId",
                table: "UserPermissions",
                type: "uuid",
                nullable: true);

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionId",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_RawPermissionId",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "DocumentsNumber",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "RawPermissionId",
                table: "UserPermissions");
        }
    }
}
