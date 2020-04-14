using Microsoft.EntityFrameworkCore.Migrations;

namespace Clc.Migrations
{
    public partial class gun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GunIp",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GunIp",
                table: "Articles");
        }
    }
}
