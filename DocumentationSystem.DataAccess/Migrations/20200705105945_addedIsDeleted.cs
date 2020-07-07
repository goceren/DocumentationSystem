using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentationSystem.DataAccess.Migrations
{
    public partial class addedIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DepartmentIsDeleted",
                table: "DocSysDepartments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentIsDeleted",
                table: "DocSysDepartments");
        }
    }
}
