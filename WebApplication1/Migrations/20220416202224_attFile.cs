using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class attFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentFileId",
                table: "Targets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Targets_AttachmentFileId",
                table: "Targets",
                column: "AttachmentFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Targets_WebFile_AttachmentFileId",
                table: "Targets",
                column: "AttachmentFileId",
                principalTable: "WebFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Targets_WebFile_AttachmentFileId",
                table: "Targets");

            migrationBuilder.DropIndex(
                name: "IX_Targets_AttachmentFileId",
                table: "Targets");

            migrationBuilder.DropColumn(
                name: "AttachmentFileId",
                table: "Targets");
        }
    }
}
