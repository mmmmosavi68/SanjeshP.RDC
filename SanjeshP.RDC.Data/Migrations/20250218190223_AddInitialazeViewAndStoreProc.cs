using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SanjeshP.RDC.Data.Migrations
{
    public partial class AddInitialazeViewAndStoreProc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var sqlFiles = assembly.GetManifestResourceNames().Where(file => file.EndsWith(".sql"));
            foreach (var sqlFile in sqlFiles)
            {
                using (Stream stream = assembly.GetManifestResourceStream(sqlFile))
                using (StreamReader reader = new StreamReader(stream))
                {
                    var sqlScript = reader.ReadToEnd();
                    migrationBuilder.Sql($"EXEC(N'{sqlScript}')");
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var command = "DROP PROCEDURE GetDepartmentSalary";
            migrationBuilder.Sql(command);
        }
    }
}