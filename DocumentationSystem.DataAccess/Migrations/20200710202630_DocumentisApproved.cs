using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentationSystem.DataAccess.Migrations
{
    public partial class DocumentisApproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DocumentIsApproved",
                table: "DocSysDocument",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentIsApproved",
                table: "DocSysDocument");
        }
    }
}
