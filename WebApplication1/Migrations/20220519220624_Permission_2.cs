using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class Permission_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentsNumber = table.Column<string>(type: "text", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermission_AvaliblePermissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "AvaliblePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermission_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserPermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AvaliblePermissionDetailId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "IX_UserPermission_PermissionId",
                table: "UserPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_UserId",
                table: "UserPermission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetail_AvaliblePermissionDetailId",
                table: "UserPermissionDetail",
                column: "AvaliblePermissionDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionDetail_UserPermissionId",
                table: "UserPermissionDetail",
                column: "UserPermissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermissionDetail");

            migrationBuilder.DropTable(
                name: "UserPermission");
        }
    }
}
