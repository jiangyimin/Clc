using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clc.Migrations
{
    public partial class clc3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_DepotFeatures_DepotId",
                table: "Workers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerType",
                table: "WorkerType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepotFeatures",
                table: "DepotFeatures");

            migrationBuilder.RenameTable(
                name: "WorkerType",
                newName: "WorkerTypes");

            migrationBuilder.RenameTable(
                name: "DepotFeatures",
                newName: "Depots");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerTypes",
                table: "WorkerTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Depots",
                table: "Depots",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Vaults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    ChannnelList = table.Column<string>(maxLength: 50, nullable: true),
                    ShiftNameList = table.Column<string>(maxLength: 50, nullable: true),
                    MinDuration = table.Column<int>(nullable: false),
                    MaxDuration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaults_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaultWorkTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    CheckinLead = table.Column<int>(nullable: false),
                    CheckinDeadline = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaultWorkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    ArticleTypeList = table.Column<string>(maxLength: 50, nullable: true),
                    ShiftNameList = table.Column<string>(maxLength: 50, nullable: true),
                    MinDuration = table.Column<int>(nullable: false),
                    MaxDuration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseWorkTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    CheckinLead = table.Column<int>(nullable: false),
                    CheckinDeadline = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseWorkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    DefaultWorkerTypeId = table.Column<int>(nullable: false),
                    Category = table.Column<string>(maxLength: 100, nullable: false),
                    Duties = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkRoles_WorkerTypes_DefaultWorkerTypeId",
                        column: x => x.DefaultWorkerTypeId,
                        principalTable: "WorkerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    CarryoutDate = table.Column<DateTime>(nullable: false),
                    WarehorseName = table.Column<string>(maxLength: 8, nullable: false),
                    WarehouseWorkTypeId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(maxLength: 4, nullable: false),
                    Remark = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseTasks_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseTasks_WarehouseWorkTypes_WarehouseWorkTypeId",
                        column: x => x.WarehouseWorkTypeId,
                        principalTable: "WarehouseWorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaultWorkTypeRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VaultWorkTypeId = table.Column<int>(nullable: false),
                    WorkRoleId = table.Column<int>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    MaxNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaultWorkTypeRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaultWorkTypeRoles_VaultWorkTypes_VaultWorkTypeId",
                        column: x => x.VaultWorkTypeId,
                        principalTable: "VaultWorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaultWorkTypeRoles_WorkRoles_WorkRoleId",
                        column: x => x.WorkRoleId,
                        principalTable: "WorkRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseWorkTypeRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WarehouseWorkTypeId = table.Column<int>(nullable: false),
                    WorkRoleId = table.Column<int>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    MaxNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseWorkTypeRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseWorkTypeRoles_WarehouseWorkTypes_WarehouseWorkTypeId",
                        column: x => x.WarehouseWorkTypeId,
                        principalTable: "WarehouseWorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseWorkTypeRoles_WorkRoles_WorkRoleId",
                        column: x => x.WorkRoleId,
                        principalTable: "WorkRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WarehouseTaskId = table.Column<int>(nullable: false),
                    EventTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Content = table.Column<string>(maxLength: 50, nullable: true),
                    Issurer = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseEvents_WarehouseTasks_WarehouseTaskId",
                        column: x => x.WarehouseTaskId,
                        principalTable: "WarehouseTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTaskWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WarehouseTaskId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false),
                    Checkin = table.Column<DateTime>(nullable: true),
                    Checkout = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseTaskWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseTaskWorkers_WarehouseTasks_WarehouseTaskId",
                        column: x => x.WarehouseTaskId,
                        principalTable: "WarehouseTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseTaskWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depots_TenantId_Cn",
                table: "Depots",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vaults_DepotId",
                table: "Vaults",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaults_TenantId_DepotId_Name",
                table: "Vaults",
                columns: new[] { "TenantId", "DepotId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaultWorkTypeRoles_VaultWorkTypeId",
                table: "VaultWorkTypeRoles",
                column: "VaultWorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VaultWorkTypeRoles_WorkRoleId",
                table: "VaultWorkTypeRoles",
                column: "WorkRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseEvents_WarehouseTaskId",
                table: "WarehouseEvents",
                column: "WarehouseTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_DepotId",
                table: "Warehouses",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_TenantId_DepotId_Name",
                table: "Warehouses",
                columns: new[] { "TenantId", "DepotId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTasks_DepotId",
                table: "WarehouseTasks",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTasks_WarehouseWorkTypeId",
                table: "WarehouseTasks",
                column: "WarehouseWorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTaskWorkers_WarehouseTaskId",
                table: "WarehouseTaskWorkers",
                column: "WarehouseTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTaskWorkers_WorkerId",
                table: "WarehouseTaskWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseWorkTypeRoles_WarehouseWorkTypeId",
                table: "WarehouseWorkTypeRoles",
                column: "WarehouseWorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseWorkTypeRoles_WorkRoleId",
                table: "WarehouseWorkTypeRoles",
                column: "WorkRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoles_DefaultWorkerTypeId",
                table: "WorkRoles",
                column: "DefaultWorkerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Depots_DepotId",
                table: "Workers",
                column: "DepotId",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Depots_DepotId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "Vaults");

            migrationBuilder.DropTable(
                name: "VaultWorkTypeRoles");

            migrationBuilder.DropTable(
                name: "WarehouseEvents");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "WarehouseTaskWorkers");

            migrationBuilder.DropTable(
                name: "WarehouseWorkTypeRoles");

            migrationBuilder.DropTable(
                name: "VaultWorkTypes");

            migrationBuilder.DropTable(
                name: "WarehouseTasks");

            migrationBuilder.DropTable(
                name: "WorkRoles");

            migrationBuilder.DropTable(
                name: "WarehouseWorkTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerTypes",
                table: "WorkerTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Depots",
                table: "Depots");

            migrationBuilder.DropIndex(
                name: "IX_Depots_TenantId_Cn",
                table: "Depots");

            migrationBuilder.RenameTable(
                name: "WorkerTypes",
                newName: "WorkerType");

            migrationBuilder.RenameTable(
                name: "Depots",
                newName: "DepotFeatures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerType",
                table: "WorkerType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepotFeatures",
                table: "DepotFeatures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_DepotFeatures_DepotId",
                table: "Workers",
                column: "DepotId",
                principalTable: "DepotFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
