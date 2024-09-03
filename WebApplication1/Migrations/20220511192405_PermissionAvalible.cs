using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class PermissionAvalible : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvaliblePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    HasEnumDetail = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliblePermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvaliblePermissionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliblePermissionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliblePermissionDetail_AvaliblePermissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "AvaliblePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvaliblePermissionDetail_PermissionId",
                table: "AvaliblePermissionDetail",
                column: "PermissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvaliblePermissionDetail");

            migrationBuilder.DropTable(
                name: "AvaliblePermissions");
        }
    }
}
