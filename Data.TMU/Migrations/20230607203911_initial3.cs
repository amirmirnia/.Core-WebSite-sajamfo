using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.TMU.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Seri",
                table: "Msc",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Well",
                table: "Msc",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "parcel",
                table: "Msc",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seri",
                table: "Msc");

            migrationBuilder.DropColumn(
                name: "Well",
                table: "Msc");

            migrationBuilder.DropColumn(
                name: "parcel",
                table: "Msc");
        }
    }
}
