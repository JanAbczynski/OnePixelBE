using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class UserPermissionfix_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserPermissionId",
                table: "UserPermissionDetails",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetails_UserPermissionId",
                table: "UserPermissionDetails",
                column: "UserPermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissionDetails_UserPermissions_UserPermissionId",
                table: "UserPermissionDetails",
                column: "UserPermissionId",
                principalTable: "UserPermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissionDetails_UserPermissions_UserPermissionId",
                table: "UserPermissionDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissionDetails_UserPermissionId",
                table: "UserPermissionDetails");

            migrationBuilder.DropColumn(
                name: "UserPermissionId",
                table: "UserPermissionDetails");
        }
    }
}
