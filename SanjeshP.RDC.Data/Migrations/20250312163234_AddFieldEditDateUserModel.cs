using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SanjeshP.RDC.Data.Migrations
{
    public partial class AddFieldEditDateUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EditDate",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditDate",
                table: "Users");
        }
    }
}
