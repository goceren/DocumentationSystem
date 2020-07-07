using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentationSystem.DataAccess.Migrations
{
    public partial class documentOpentoEveryone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DocumentOpentoEveryone",
                table: "DocSysDocument",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentOpentoEveryone",
                table: "DocSysDocument");
        }
    }
}
