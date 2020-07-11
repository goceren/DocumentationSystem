using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentationSystem.DataAccess.Migrations
{
    public partial class SubTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "DocumentSubtitle",
                table: "DocSysDocument",
                nullable: false,
                defaultValue: "");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
