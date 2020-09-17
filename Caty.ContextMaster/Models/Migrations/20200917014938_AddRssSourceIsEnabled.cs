using Microsoft.EntityFrameworkCore.Migrations;

namespace Caty.ContextMaster.Models.Migrations
{
    public partial class AddRssSourceIsEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "RssSources",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "RssSources");
        }
    }
}
