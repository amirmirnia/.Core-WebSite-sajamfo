using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.TMU.Migrations
{
    public partial class initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "statusstatusface",
                table: "mscPercents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdMSC",
                table: "AndicatorUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "statusstatusface",
                table: "mscPercents");

            migrationBuilder.DropColumn(
                name: "IdMSC",
                table: "AndicatorUsers");
        }
    }
}
