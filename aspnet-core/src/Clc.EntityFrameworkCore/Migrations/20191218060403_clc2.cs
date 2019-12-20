using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clc.Migrations
{
    public partial class clc2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteInBoxes_BoxRecords_BoxRecordId",
                table: "RouteInBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteOutBoxes_BoxRecords_BoxRecordId",
                table: "RouteOutBoxes");

            migrationBuilder.AddColumn<string>(
                name: "Rated",
                table: "RouteTasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReportDate",
                table: "Depots",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InTime",
                table: "BoxRecords",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliverTime",
                table: "BoxRecords",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InRouteTaskId",
                table: "BoxRecords",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutRouteTaskId",
                table: "BoxRecords",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupTime",
                table: "BoxRecords",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerTaskTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    TaskTypeId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    Remark = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTaskTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerTaskTypes_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerTaskTypes_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerTaskTypes_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasStations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 4, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    DepotList = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OilTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 1, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OilTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreVehicleWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    VehicleId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false),
                    WorkRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreVehicleWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreVehicleWorkers_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreVehicleWorkers_WorkRoles_WorkRoleId",
                        column: x => x.WorkRoleId,
                        principalTable: "WorkRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreVehicleWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleMTTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 1, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleMTTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OilRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateWorkerId = table.Column<int>(nullable: false),
                    VehicleId = table.Column<int>(nullable: false),
                    GasStationId = table.Column<int>(nullable: false),
                    OilTypeId = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Mileage = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(maxLength: 512, nullable: true),
                    ConfirmTime = table.Column<DateTime>(nullable: true),
                    ProcessWorkerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OilRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OilRecords_Workers_CreateWorkerId",
                        column: x => x.CreateWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OilRecords_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalTable: "GasStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OilRecords_OilTypes_OilTypeId",
                        column: x => x.OilTypeId,
                        principalTable: "OilTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OilRecords_Workers_ProcessWorkerId",
                        column: x => x.ProcessWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OilRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleMTMTRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateWorkerId = table.Column<int>(nullable: false),
                    VehicleId = table.Column<int>(nullable: false),
                    VehicleMTTypeId = table.Column<int>(nullable: false),
                    MTDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Content = table.Column<string>(maxLength: 512, nullable: false),
                    Remark = table.Column<string>(maxLength: 512, nullable: true),
                    ConfirmTime = table.Column<DateTime>(nullable: true),
                    ProcessWorkerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleMTMTRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleMTMTRecords_Workers_CreateWorkerId",
                        column: x => x.CreateWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleMTMTRecords_Workers_ProcessWorkerId",
                        column: x => x.ProcessWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleMTMTRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleMTMTRecords_VehicleMTTypes_VehicleMTTypeId",
                        column: x => x.VehicleMTTypeId,
                        principalTable: "VehicleMTTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxRecords_InRouteTaskId",
                table: "BoxRecords",
                column: "InRouteTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxRecords_OutRouteTaskId",
                table: "BoxRecords",
                column: "OutRouteTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTaskTypes_DepotId",
                table: "CustomerTaskTypes",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTaskTypes_TaskTypeId",
                table: "CustomerTaskTypes",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTaskTypes_CustomerId_TaskTypeId_DepotId",
                table: "CustomerTaskTypes",
                columns: new[] { "CustomerId", "TaskTypeId", "DepotId" },
                unique: true,
                filter: "[DepotId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GasStations_TenantId_Cn",
                table: "GasStations",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OilRecords_CreateWorkerId",
                table: "OilRecords",
                column: "CreateWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_OilRecords_GasStationId",
                table: "OilRecords",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_OilRecords_OilTypeId",
                table: "OilRecords",
                column: "OilTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OilRecords_ProcessWorkerId",
                table: "OilRecords",
                column: "ProcessWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_OilRecords_VehicleId",
                table: "OilRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_OilRecords_TenantId_VehicleId",
                table: "OilRecords",
                columns: new[] { "TenantId", "VehicleId" });

            migrationBuilder.CreateIndex(
                name: "IX_OilRecords_TenantId_CreateTime_VehicleId",
                table: "OilRecords",
                columns: new[] { "TenantId", "CreateTime", "VehicleId" });

            migrationBuilder.CreateIndex(
                name: "IX_PreVehicleWorkers_VehicleId",
                table: "PreVehicleWorkers",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreVehicleWorkers_WorkRoleId",
                table: "PreVehicleWorkers",
                column: "WorkRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreVehicleWorkers_WorkerId",
                table: "PreVehicleWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMTMTRecords_CreateWorkerId",
                table: "VehicleMTMTRecords",
                column: "CreateWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMTMTRecords_ProcessWorkerId",
                table: "VehicleMTMTRecords",
                column: "ProcessWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMTMTRecords_VehicleId",
                table: "VehicleMTMTRecords",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMTMTRecords_VehicleMTTypeId",
                table: "VehicleMTMTRecords",
                column: "VehicleMTTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMTMTRecords_TenantId_VehicleId",
                table: "VehicleMTMTRecords",
                columns: new[] { "TenantId", "VehicleId" });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMTMTRecords_TenantId_CreateTime_VehicleId",
                table: "VehicleMTMTRecords",
                columns: new[] { "TenantId", "CreateTime", "VehicleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BoxRecords_RouteTasks_InRouteTaskId",
                table: "BoxRecords",
                column: "InRouteTaskId",
                principalTable: "RouteTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxRecords_RouteTasks_OutRouteTaskId",
                table: "BoxRecords",
                column: "OutRouteTaskId",
                principalTable: "RouteTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RouteInBoxes_BoxRecords_BoxRecordId",
                table: "RouteInBoxes",
                column: "BoxRecordId",
                principalTable: "BoxRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RouteOutBoxes_BoxRecords_BoxRecordId",
                table: "RouteOutBoxes",
                column: "BoxRecordId",
                principalTable: "BoxRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoxRecords_RouteTasks_InRouteTaskId",
                table: "BoxRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_BoxRecords_RouteTasks_OutRouteTaskId",
                table: "BoxRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteInBoxes_BoxRecords_BoxRecordId",
                table: "RouteInBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteOutBoxes_BoxRecords_BoxRecordId",
                table: "RouteOutBoxes");

            migrationBuilder.DropTable(
                name: "CustomerTaskTypes");

            migrationBuilder.DropTable(
                name: "OilRecords");

            migrationBuilder.DropTable(
                name: "PreVehicleWorkers");

            migrationBuilder.DropTable(
                name: "VehicleMTMTRecords");

            migrationBuilder.DropTable(
                name: "GasStations");

            migrationBuilder.DropTable(
                name: "OilTypes");

            migrationBuilder.DropTable(
                name: "VehicleMTTypes");

            migrationBuilder.DropIndex(
                name: "IX_BoxRecords_InRouteTaskId",
                table: "BoxRecords");

            migrationBuilder.DropIndex(
                name: "IX_BoxRecords_OutRouteTaskId",
                table: "BoxRecords");

            migrationBuilder.DropColumn(
                name: "Rated",
                table: "RouteTasks");

            migrationBuilder.DropColumn(
                name: "LastReportDate",
                table: "Depots");

            migrationBuilder.DropColumn(
                name: "DeliverTime",
                table: "BoxRecords");

            migrationBuilder.DropColumn(
                name: "InRouteTaskId",
                table: "BoxRecords");

            migrationBuilder.DropColumn(
                name: "OutRouteTaskId",
                table: "BoxRecords");

            migrationBuilder.DropColumn(
                name: "PickupTime",
                table: "BoxRecords");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InTime",
                table: "BoxRecords",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RouteInBoxes_BoxRecords_BoxRecordId",
                table: "RouteInBoxes",
                column: "BoxRecordId",
                principalTable: "BoxRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RouteOutBoxes_BoxRecords_BoxRecordId",
                table: "RouteOutBoxes",
                column: "BoxRecordId",
                principalTable: "BoxRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
