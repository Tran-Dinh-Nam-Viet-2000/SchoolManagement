using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateFKforTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Classes",
                newName: "ClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Classes",
                newName: "Id");
        }
    }
}
