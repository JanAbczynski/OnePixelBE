using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class UserPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_AvaliblePermissions_PermissionId",
                table: "UserPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_Users_UserId",
                table: "UserPermission");

            migrationBuilder.DropTable(
                name: "UserPermissionDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermission",
                table: "UserPermission");

            migrationBuilder.DropColumn(
                name: "DocumentsNumber",
                table: "UserPermission");

            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "UserPermission");

            migrationBuilder.RenameTable(
                name: "UserPermission",
                newName: "UserPermissions");

            migrationBuilder.RenameColumn(
                name: "PermissionId",
                table: "UserPermissions",
                newName: "RawPermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermission_UserId",
                table: "UserPermissions",
                newName: "IX_UserPermissions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermission_PermissionId",
                table: "UserPermissions",
                newName: "IX_UserPermissions_RawPermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionId",
                table: "UserPermissions",
                column: "RawPermissionId",
                principalTable: "AvaliblePermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_Users_UserId",
                table: "UserPermissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_AvaliblePermissions_RawPermissionId",
                table: "UserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_Users_UserId",
                table: "UserPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermissions",
                table: "UserPermissions");

            migrationBuilder.RenameTable(
                name: "UserPermissions",
                newName: "UserPermission");

            migrationBuilder.RenameColumn(
                name: "RawPermissionId",
                table: "UserPermission",
                newName: "PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermissions_UserId",
                table: "UserPermission",
                newName: "IX_UserPermission_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermissions_RawPermissionId",
                table: "UserPermission",
                newName: "IX_UserPermission_PermissionId");

            migrationBuilder.AddColumn<string>(
                name: "DocumentsNumber",
                table: "UserPermission",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "UserPermission",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermission",
                table: "UserPermission",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserPermissionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AvaliblePermissionDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserPermissionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissionDetail_AvaliblePermissionDetail_AvaliblePermi~",
                        column: x => x.AvaliblePermissionDetailId,
                        principalTable: "AvaliblePermissionDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissionDetail_UserPermission_UserPermissionId",
                        column: x => x.UserPermissionId,
                        principalTable: "UserPermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetail_AvaliblePermissionDetailId",
                table: "UserPermissionDetail",
                column: "AvaliblePermissionDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetail_UserPermissionId",
                table: "UserPermissionDetail",
                column: "UserPermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_AvaliblePermissions_PermissionId",
                table: "UserPermission",
                column: "PermissionId",
                principalTable: "AvaliblePermissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_Users_UserId",
                table: "UserPermission",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
