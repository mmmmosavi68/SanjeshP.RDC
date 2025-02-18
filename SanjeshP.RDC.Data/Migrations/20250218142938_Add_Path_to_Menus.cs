using Microsoft.EntityFrameworkCore.Migrations;

namespace SanjeshP.RDC.Data.Migrations
{
    public partial class Add_Path_to_Menus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Menus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Menus");
        }
    }
}
