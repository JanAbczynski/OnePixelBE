using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class CustomFileDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Targets_CustomFile_FileId",
                table: "Targets");

            migrationBuilder.DropTable(
                name: "CustomFile");

            migrationBuilder.DropIndex(
                name: "IX_Targets_FileId",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Targets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Targets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileFolder = table.Column<string>(type: "text", nullable: true),
                    FileUrl = table.Column<string>(type: "text", nullable: true),
                    OriginalName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFile", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Targets_FileId",
                table: "Targets",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_CustomFile_FileId",
                table: "Targets",
                column: "FileId",
                principalTable: "CustomFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
