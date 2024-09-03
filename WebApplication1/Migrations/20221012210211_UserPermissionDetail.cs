using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class UserPermissionDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPermissionDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserPermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    RawPermissionDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissionDetails_AvaliblePermissionDetail_RawPermissio~",
                        column: x => x.RawPermissionDetailId,
                        principalTable: "AvaliblePermissionDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissionDetails_AvaliblePermissions_UserPermissionId",
                        column: x => x.UserPermissionId,
                        principalTable: "AvaliblePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissionDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetails_RawPermissionDetailId",
                table: "UserPermissionDetails",
                column: "RawPermissionDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetails_UserId",
                table: "UserPermissionDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetails_UserPermissionId",
                table: "UserPermissionDetails",
                column: "UserPermissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermissionDetails");
        }
    }
}
