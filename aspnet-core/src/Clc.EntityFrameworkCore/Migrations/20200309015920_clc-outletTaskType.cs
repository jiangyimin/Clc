using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clc.Migrations
{
    public partial class clcoutletTaskType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutletTaskTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OutletId = table.Column<int>(nullable: false),
                    TaskTypeId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    Remark = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutletTaskTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutletTaskTypes_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutletTaskTypes_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutletTaskTypes_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutletTaskTypes_DepotId",
                table: "OutletTaskTypes",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_OutletTaskTypes_TaskTypeId",
                table: "OutletTaskTypes",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutletTaskTypes_OutletId_TaskTypeId_DepotId",
                table: "OutletTaskTypes",
                columns: new[] { "OutletId", "TaskTypeId", "DepotId" },
                unique: true,
                filter: "[DepotId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutletTaskTypes");
        }
    }
}
