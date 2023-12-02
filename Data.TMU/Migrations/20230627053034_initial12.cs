using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.TMU.Migrations
{
    public partial class initial12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MscUsers_mscPercents_MscPercentId",
                table: "MscUsers");

            migrationBuilder.DropIndex(
                name: "IX_MscUsers_MscPercentId",
                table: "MscUsers");

            migrationBuilder.DropColumn(
                name: "MscPercentId",
                table: "MscUsers");

            migrationBuilder.AddColumn<int>(
                name: "IdAU",
                table: "MscUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdAU",
                table: "MscUsers");

            migrationBuilder.AddColumn<int>(
                name: "MscPercentId",
                table: "MscUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MscUsers_MscPercentId",
                table: "MscUsers",
                column: "MscPercentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MscUsers_mscPercents_MscPercentId",
                table: "MscUsers",
                column: "MscPercentId",
                principalTable: "mscPercents",
                principalColumn: "Id");
        }
    }
}
