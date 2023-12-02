using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.TMU.Migrations
{
    public partial class initial36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "statusfaceprint",
                columns: table => new
                {
                    Idmsc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    activity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numberEblagh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateEblagh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    volume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    volumeunit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deadline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    listpersentUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    listpersent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullnameReciver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateConfraim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leternumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusfaceprint", x => x.Idmsc);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "statusfaceprint");
        }
    }
}
