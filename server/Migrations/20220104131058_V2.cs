using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servers_Datacenters_DatacenterID",
                table: "Servers");

            migrationBuilder.DropTable(
                name: "Datacenters");

            migrationBuilder.DropIndex(
                name: "IX_Servers_DatacenterID",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "DatacenterID",
                table: "Servers");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "Users",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "Users",
                type: "nvarchar(72)",
                maxLength: 72,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(72)",
                oldMaxLength: 72);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "processor",
                table: "Servers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "Users",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "Users",
                type: "nvarchar(72)",
                maxLength: 72,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(72)",
                oldMaxLength: 72,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "processor",
                table: "Servers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DatacenterID",
                table: "Servers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Datacenters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datacenters", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servers_DatacenterID",
                table: "Servers",
                column: "DatacenterID");

            migrationBuilder.AddForeignKey(
                name: "FK_Servers_Datacenters_DatacenterID",
                table: "Servers",
                column: "DatacenterID",
                principalTable: "Datacenters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
