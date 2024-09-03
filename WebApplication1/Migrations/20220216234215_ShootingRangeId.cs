using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnePixelBE.Migrations
{
    public partial class ShootingRangeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OneRanges_ShootingRanges_ShootingRangeId",
                table: "OneRanges");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShootingRangeId",
                table: "OneRanges",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OneRanges_ShootingRanges_ShootingRangeId",
                table: "OneRanges",
                column: "ShootingRangeId",
                principalTable: "ShootingRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OneRanges_ShootingRanges_ShootingRangeId",
                table: "OneRanges");

            migrationBuilder.AlterColumn<Guid>(
                name: "ShootingRangeId",
                table: "OneRanges",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_OneRanges_ShootingRanges_ShootingRangeId",
                table: "OneRanges",
                column: "ShootingRangeId",
                principalTable: "ShootingRanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
