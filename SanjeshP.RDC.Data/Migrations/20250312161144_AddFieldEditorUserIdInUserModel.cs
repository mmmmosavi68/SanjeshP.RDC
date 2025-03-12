using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SanjeshP.RDC.Data.Migrations
{
    public partial class AddFieldEditorUserIdInUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatorUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatorUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "EditorUserId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EditorUserId",
                table: "Users",
                column: "EditorUserId",
                unique: true,
                filter: "[EditorUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_EditorUserId",
                table: "Users",
                column: "EditorUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_EditorUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EditorUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EditorUserId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorUserId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatorUserId",
                table: "Users",
                column: "CreatorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatorUserId",
                table: "Users",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
