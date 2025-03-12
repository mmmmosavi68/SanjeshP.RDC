using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SanjeshP.RDC.Data.Migrations
{
    public partial class AddFieldEditorCreatorUserIdInUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_EditorUserId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorUserId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatorUserId",
                table: "Users",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EditorUserId",
                table: "Users",
                column: "EditorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatorUserId",
                table: "Users",
                column: "CreatorUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatorUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatorUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EditorUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EditorUserId",
                table: "Users",
                column: "EditorUserId",
                unique: true,
                filter: "[EditorUserId] IS NOT NULL");
        }
    }
}
