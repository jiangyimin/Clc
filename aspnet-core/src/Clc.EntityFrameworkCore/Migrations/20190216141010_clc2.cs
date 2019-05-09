using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clc.Migrations
{
    public partial class clc2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_WorkerTypes_WorkerTypeId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "WorkerTypes");

            migrationBuilder.DropIndex(
                name: "IX_Workers_WorkerTypeId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "WorkerTypeId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "BasicPrice",
                table: "TaskTypes");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "TaskTypes");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Workers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwinUserName",
                table: "AbpRoles",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwinUserPassword",
                table: "AbpRoles",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArticleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    BindStyle = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ATMTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATMTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepotFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    UseRouteForIdentify = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepotFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_DepotId",
                table: "Workers",
                column: "DepotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_DepotFeatures_DepotId",
                table: "Workers",
                column: "DepotId",
                principalTable: "DepotFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_DepotFeatures_DepotId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "ArticleTypes");

            migrationBuilder.DropTable(
                name: "ATMTypes");

            migrationBuilder.DropTable(
                name: "DepotFeatures");

            migrationBuilder.DropTable(
                name: "WorkerType");

            migrationBuilder.DropIndex(
                name: "IX_Workers_DepotId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "TwinUserName",
                table: "AbpRoles");

            migrationBuilder.DropColumn(
                name: "TwinUserPassword",
                table: "AbpRoles");

            migrationBuilder.AddColumn<int>(
                name: "WorkerTypeId",
                table: "Workers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BasicPrice",
                table: "TaskTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "TaskTypes",
                maxLength: 8,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cn = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_WorkerTypeId",
                table: "Workers",
                column: "WorkerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_WorkerTypes_WorkerTypeId",
                table: "Workers",
                column: "WorkerTypeId",
                principalTable: "WorkerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
