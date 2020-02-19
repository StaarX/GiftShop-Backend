using Microsoft.EntityFrameworkCore.Migrations;

namespace SS.Template.Persistence.Migrations
{
    public partial class ImgToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgSource",
                table: "Product",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgSource",
                table: "Product");
        }
    }
}
