using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class UserPermissionfix_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissionDetails_AvaliblePermissions_UserPermissionId",
                table: "UserPermissionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissionDetails_Users_UserId",
                table: "UserPermissionDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissionDetails_UserId",
                table: "UserPermissionDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissionDetails_UserPermissionId",
                table: "UserPermissionDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserPermissionDetails");

            migrationBuilder.DropColumn(
                name: "UserPermissionId",
                table: "UserPermissionDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserPermissionDetails",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserPermissionId",
                table: "UserPermissionDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetails_UserId",
                table: "UserPermissionDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetails_UserPermissionId",
                table: "UserPermissionDetails",
                column: "UserPermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissionDetails_AvaliblePermissions_UserPermissionId",
                table: "UserPermissionDetails",
                column: "UserPermissionId",
                principalTable: "AvaliblePermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissionDetails_Users_UserId",
                table: "UserPermissionDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
