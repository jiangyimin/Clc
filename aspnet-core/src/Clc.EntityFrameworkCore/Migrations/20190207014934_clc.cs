using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clc.Migrations
{
    public partial class clc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    Mobile = table.Column<string>(maxLength: 11, nullable: true),
                    RoleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Managers_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    GroupName = table.Column<string>(maxLength: 8, nullable: true),
                    BasicPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerTypes",
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
                    table.PrimaryKey("PK_WorkerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 8, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    WorkerTypeId = table.Column<int>(nullable: false),
                    Password = table.Column<string>(maxLength: 10, nullable: true),
                    Photo = table.Column<byte[]>(nullable: true),
                    Finger = table.Column<string>(maxLength: 1024, nullable: true),
                    IDNumber = table.Column<string>(maxLength: 18, nullable: true),
                    IDCardNo = table.Column<string>(maxLength: 18, nullable: true),
                    Mobile = table.Column<string>(maxLength: 11, nullable: true),
                    DeviceId = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_WorkerTypes_WorkerTypeId",
                        column: x => x.WorkerTypeId,
                        principalTable: "WorkerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Managers_RoleId",
                table: "Managers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_WorkerTypeId",
                table: "Workers",
                column: "WorkerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_TenantId_Cn",
                table: "Workers",
                columns: new[] { "TenantId", "Cn" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "WorkerTypes");
        }
    }
}
