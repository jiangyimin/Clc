using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clc.Migrations
{
    public partial class clc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWorkerRole",
                table: "AbpRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ArticleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 1, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    BindStyle = table.Column<string>(maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 4, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Contact = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Depots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Radius = table.Column<int>(nullable: true),
                    ActiveRouteNeedCheckin = table.Column<bool>(nullable: false),
                    UnlockScreenPassword = table.Column<string>(maxLength: 8, nullable: true),
                    ReportTo = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
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
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    WorkRoles = table.Column<string>(maxLength: 50, nullable: false),
                    LendArticleLead = table.Column<int>(nullable: false),
                    LendArticleDeadline = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteTypes", x => x.Id);
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
                    isTemporary = table.Column<bool>(nullable: false),
                    BasicPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outlets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 6, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 6, nullable: true),
                    Ciphertext = table.Column<string>(maxLength: 6, nullable: true),
                    Contact = table.Column<string>(maxLength: 50, nullable: true),
                    Weixins = table.Column<string>(maxLength: 50, nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Latitude = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outlets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outlets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 8, nullable: false),
                    License = table.Column<string>(maxLength: 7, nullable: false),
                    Photo = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workplaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    WorkRoles = table.Column<string>(maxLength: 50, nullable: false),
                    ArticleTypeList = table.Column<string>(maxLength: 50, nullable: true),
                    ShareDepotList = table.Column<string>(maxLength: 50, nullable: true),
                    MinDuration = table.Column<int>(nullable: false),
                    MaxDuration = table.Column<int>(nullable: false),
                    DoorIp = table.Column<string>(nullable: true),
                    CameraIp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workplaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workplaces_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    PostId = table.Column<int>(nullable: false),
                    Password = table.Column<string>(maxLength: 10, nullable: true),
                    WorkerRoleName = table.Column<string>(maxLength: 32, nullable: true),
                    Rfid = table.Column<string>(maxLength: 18, nullable: true),
                    Photo = table.Column<byte[]>(nullable: true),
                    DeviceId = table.Column<string>(maxLength: 50, nullable: true),
                    AdditiveInfo = table.Column<string>(maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workers_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Workers_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    DefaultPostId = table.Column<int>(nullable: true),
                    Duties = table.Column<string>(maxLength: 50, nullable: true),
                    ArticleTypeList = table.Column<string>(maxLength: 50, nullable: true),
                    mustHave = table.Column<bool>(nullable: false),
                    MaxNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkRoles_Posts_DefaultPostId",
                        column: x => x.DefaultPostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    RouteName = table.Column<string>(maxLength: 20, nullable: false),
                    RouteTypeId = table.Column<int>(nullable: false),
                    VehicleId = table.Column<int>(nullable: true),
                    StartTime = table.Column<string>(maxLength: 5, nullable: false),
                    EndTime = table.Column<string>(maxLength: 5, nullable: false),
                    Mileage = table.Column<float>(nullable: true),
                    Remark = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreRoutes_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreRoutes_RouteTypes_RouteTypeId",
                        column: x => x.RouteTypeId,
                        principalTable: "RouteTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreRoutes_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Affairs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    CarryoutDate = table.Column<DateTime>(nullable: false),
                    WorkplaceId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 2, nullable: false),
                    StartTime = table.Column<string>(maxLength: 5, nullable: false),
                    EndTime = table.Column<string>(maxLength: 5, nullable: false),
                    IsTomorrow = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 50, nullable: true),
                    CreateWorkerId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Affairs_Workers_CreateWorkerId",
                        column: x => x.CreateWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Affairs_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Affairs_Workplaces_WorkplaceId",
                        column: x => x.WorkplaceId,
                        principalTable: "Workplaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    CarryoutDate = table.Column<DateTime>(nullable: false),
                    RouteName = table.Column<string>(maxLength: 20, nullable: false),
                    VehicleId = table.Column<int>(nullable: true),
                    RouteTypeId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 2, nullable: false),
                    StartTime = table.Column<string>(maxLength: 5, nullable: false),
                    EndTime = table.Column<string>(maxLength: 5, nullable: false),
                    Mileage = table.Column<float>(nullable: true),
                    Remark = table.Column<string>(maxLength: 50, nullable: true),
                    CreateWorkerId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    SetoutTime = table.Column<DateTime>(nullable: false),
                    ReturnTime = table.Column<DateTime>(nullable: false),
                    ActualMileage = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Workers_CreateWorkerId",
                        column: x => x.CreateWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Routes_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Routes_RouteTypes_RouteTypeId",
                        column: x => x.RouteTypeId,
                        principalTable: "RouteTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Routes_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Signins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    CarryoutDate = table.Column<DateTime>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false),
                    SigninTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signins_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Signins_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PreRouteTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    PreRouteId = table.Column<int>(nullable: false),
                    ArriveTime = table.Column<string>(maxLength: 5, nullable: false),
                    OutletId = table.Column<int>(nullable: false),
                    TaskTypeId = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRouteTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreRouteTasks_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreRouteTasks_PreRoutes_PreRouteId",
                        column: x => x.PreRouteId,
                        principalTable: "PreRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreRouteTasks_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreRouteWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    PreRouteId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false),
                    WorkRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRouteWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreRouteWorkers_PreRoutes_PreRouteId",
                        column: x => x.PreRouteId,
                        principalTable: "PreRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreRouteWorkers_WorkRoles_WorkRoleId",
                        column: x => x.WorkRoleId,
                        principalTable: "WorkRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreRouteWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AffairEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    AffairId = table.Column<int>(nullable: false),
                    EventTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Issurer = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffairEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffairEvents_Affairs_AffairId",
                        column: x => x.AffairId,
                        principalTable: "Affairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AffairTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    AffairId = table.Column<int>(nullable: false),
                    WorkplaceId = table.Column<int>(nullable: false),
                    StartTime = table.Column<string>(maxLength: 5, nullable: false),
                    EndTime = table.Column<string>(maxLength: 5, nullable: false),
                    IsTomorrow = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 50, nullable: true),
                    CreateWorkerId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffairTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffairTasks_Affairs_AffairId",
                        column: x => x.AffairId,
                        principalTable: "Affairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AffairTasks_Workers_CreateWorkerId",
                        column: x => x.CreateWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AffairTasks_Workplaces_WorkplaceId",
                        column: x => x.WorkplaceId,
                        principalTable: "Workplaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AffairWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    AffairId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false),
                    WorkRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffairWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffairWorkers_Affairs_AffairId",
                        column: x => x.AffairId,
                        principalTable: "Affairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AffairWorkers_WorkRoles_WorkRoleId",
                        column: x => x.WorkRoleId,
                        principalTable: "WorkRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AffairWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoorRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    WorkplaceId = table.Column<int>(nullable: false),
                    OpenAffairId = table.Column<int>(nullable: false),
                    ApplyAffairId = table.Column<int>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoorRecords_Affairs_ApplyAffairId",
                        column: x => x.ApplyAffairId,
                        principalTable: "Affairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoorRecords_Affairs_OpenAffairId",
                        column: x => x.OpenAffairId,
                        principalTable: "Affairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoorRecords_Workplaces_WorkplaceId",
                        column: x => x.WorkplaceId,
                        principalTable: "Workplaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    RouteId = table.Column<int>(nullable: false),
                    EventTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Issurer = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteEvents_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    RouteId = table.Column<int>(nullable: false),
                    ArriveTime = table.Column<string>(maxLength: 5, nullable: false),
                    OutletId = table.Column<int>(nullable: false),
                    TaskTypeId = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 50, nullable: true),
                    CreateWorkerId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IdentifyTime = table.Column<DateTime>(nullable: true),
                    OutletIdentifyInfo = table.Column<string>(maxLength: 50, nullable: true),
                    Price = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteTasks_Workers_CreateWorkerId",
                        column: x => x.CreateWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteTasks_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteTasks_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteTasks_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    RouteId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false),
                    WorkRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteWorkers_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteWorkers_WorkRoles_WorkRoleId",
                        column: x => x.WorkRoleId,
                        principalTable: "WorkRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    ArticleTypeId = table.Column<int>(nullable: false),
                    Rfid = table.Column<string>(maxLength: 20, nullable: true),
                    BindInfo = table.Column<string>(maxLength: 20, nullable: true),
                    ArticleRecordId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_ArticleTypes_ArticleTypeId",
                        column: x => x.ArticleTypeId,
                        principalTable: "ArticleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Articles_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    ArticleId = table.Column<int>(nullable: false),
                    RouteWorkerId = table.Column<int>(nullable: false),
                    LendTime = table.Column<DateTime>(nullable: false),
                    ReturnTime = table.Column<DateTime>(nullable: true),
                    LendWorkers = table.Column<string>(maxLength: 64, nullable: false),
                    ReturnWorkers = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleRecords_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleRecords_RouteWorkers_RouteWorkerId",
                        column: x => x.RouteWorkerId,
                        principalTable: "RouteWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RouteArticles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    RouteId = table.Column<int>(nullable: false),
                    RouteWorkerId = table.Column<int>(nullable: false),
                    ArticleRecordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteArticles_ArticleRecords_ArticleRecordId",
                        column: x => x.ArticleRecordId,
                        principalTable: "ArticleRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteArticles_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteArticles_RouteWorkers_RouteWorkerId",
                        column: x => x.RouteWorkerId,
                        principalTable: "RouteWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoxRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    BoxId = table.Column<int>(nullable: false),
                    InTime = table.Column<DateTime>(nullable: false),
                    OutTime = table.Column<DateTime>(nullable: true),
                    InWorkers = table.Column<string>(maxLength: 64, nullable: false),
                    OutWorkers = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    OutletId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Ramark = table.Column<string>(maxLength: 50, nullable: true),
                    BoxRecordId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boxes_BoxRecords_BoxRecordId",
                        column: x => x.BoxRecordId,
                        principalTable: "BoxRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Boxes_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RouteInBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    RouteId = table.Column<int>(nullable: false),
                    RouteTaskId = table.Column<int>(nullable: false),
                    BoxRecordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteInBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteInBoxes_BoxRecords_BoxRecordId",
                        column: x => x.BoxRecordId,
                        principalTable: "BoxRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteInBoxes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteInBoxes_RouteTasks_RouteTaskId",
                        column: x => x.RouteTaskId,
                        principalTable: "RouteTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteOutBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    RouteId = table.Column<int>(nullable: false),
                    RouteTaskId = table.Column<int>(nullable: false),
                    BoxRecordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteOutBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteOutBoxes_BoxRecords_BoxRecordId",
                        column: x => x.BoxRecordId,
                        principalTable: "BoxRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteOutBoxes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RouteOutBoxes_RouteTasks_RouteTaskId",
                        column: x => x.RouteTaskId,
                        principalTable: "RouteTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_WorkerId",
                table: "AbpUsers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_AffairEvents_AffairId",
                table: "AffairEvents",
                column: "AffairId");

            migrationBuilder.CreateIndex(
                name: "IX_Affairs_CreateWorkerId",
                table: "Affairs",
                column: "CreateWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Affairs_DepotId",
                table: "Affairs",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Affairs_WorkplaceId",
                table: "Affairs",
                column: "WorkplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Affairs_TenantId_CarryoutDate_DepotId",
                table: "Affairs",
                columns: new[] { "TenantId", "CarryoutDate", "DepotId" });

            migrationBuilder.CreateIndex(
                name: "IX_AffairTasks_AffairId",
                table: "AffairTasks",
                column: "AffairId");

            migrationBuilder.CreateIndex(
                name: "IX_AffairTasks_CreateWorkerId",
                table: "AffairTasks",
                column: "CreateWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_AffairTasks_WorkplaceId",
                table: "AffairTasks",
                column: "WorkplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_AffairWorkers_AffairId",
                table: "AffairWorkers",
                column: "AffairId");

            migrationBuilder.CreateIndex(
                name: "IX_AffairWorkers_WorkRoleId",
                table: "AffairWorkers",
                column: "WorkRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AffairWorkers_WorkerId",
                table: "AffairWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRecords_ArticleId",
                table: "ArticleRecords",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRecords_RouteWorkerId",
                table: "ArticleRecords",
                column: "RouteWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleRecords_TenantId_LendTime",
                table: "ArticleRecords",
                columns: new[] { "TenantId", "LendTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleRecordId",
                table: "Articles",
                column: "ArticleRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleTypeId",
                table: "Articles",
                column: "ArticleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_DepotId",
                table: "Articles",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_TenantId_Cn",
                table: "Articles",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_BoxRecordId",
                table: "Boxes",
                column: "BoxRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_OutletId",
                table: "Boxes",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_TenantId_Cn",
                table: "Boxes",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoxRecords_BoxId",
                table: "BoxRecords",
                column: "BoxId");

            migrationBuilder.CreateIndex(
                name: "IX_BoxRecords_TenantId_InTime",
                table: "BoxRecords",
                columns: new[] { "TenantId", "InTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId_Cn",
                table: "Customers",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Depots_TenantId_Cn",
                table: "Depots",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoorRecords_ApplyAffairId",
                table: "DoorRecords",
                column: "ApplyAffairId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorRecords_OpenAffairId",
                table: "DoorRecords",
                column: "OpenAffairId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorRecords_WorkplaceId",
                table: "DoorRecords",
                column: "WorkplaceId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorRecords_TenantId_CreateTime",
                table: "DoorRecords",
                columns: new[] { "TenantId", "CreateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_DoorRecords_TenantId_WorkplaceId",
                table: "DoorRecords",
                columns: new[] { "TenantId", "WorkplaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_CustomerId",
                table: "Outlets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_TenantId_Cn",
                table: "Outlets",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreRoutes_DepotId",
                table: "PreRoutes",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRoutes_RouteTypeId",
                table: "PreRoutes",
                column: "RouteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRoutes_VehicleId",
                table: "PreRoutes",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRoutes_TenantId_DepotId_RouteName",
                table: "PreRoutes",
                columns: new[] { "TenantId", "DepotId", "RouteName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreRouteTasks_OutletId",
                table: "PreRouteTasks",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRouteTasks_PreRouteId",
                table: "PreRouteTasks",
                column: "PreRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRouteTasks_TaskTypeId",
                table: "PreRouteTasks",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRouteWorkers_PreRouteId",
                table: "PreRouteWorkers",
                column: "PreRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRouteWorkers_WorkRoleId",
                table: "PreRouteWorkers",
                column: "WorkRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRouteWorkers_WorkerId",
                table: "PreRouteWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteArticles_ArticleRecordId",
                table: "RouteArticles",
                column: "ArticleRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteArticles_RouteId",
                table: "RouteArticles",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteArticles_RouteWorkerId",
                table: "RouteArticles",
                column: "RouteWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteEvents_RouteId",
                table: "RouteEvents",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteInBoxes_BoxRecordId",
                table: "RouteInBoxes",
                column: "BoxRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteInBoxes_RouteId",
                table: "RouteInBoxes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteInBoxes_RouteTaskId",
                table: "RouteInBoxes",
                column: "RouteTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteOutBoxes_BoxRecordId",
                table: "RouteOutBoxes",
                column: "BoxRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteOutBoxes_RouteId",
                table: "RouteOutBoxes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteOutBoxes_RouteTaskId",
                table: "RouteOutBoxes",
                column: "RouteTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_CreateWorkerId",
                table: "Routes",
                column: "CreateWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_DepotId",
                table: "Routes",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_RouteTypeId",
                table: "Routes",
                column: "RouteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_VehicleId",
                table: "Routes",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TenantId_CarryoutDate_DepotId_RouteName",
                table: "Routes",
                columns: new[] { "TenantId", "CarryoutDate", "DepotId", "RouteName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteTasks_CreateWorkerId",
                table: "RouteTasks",
                column: "CreateWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteTasks_OutletId",
                table: "RouteTasks",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteTasks_RouteId",
                table: "RouteTasks",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteTasks_TaskTypeId",
                table: "RouteTasks",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteWorkers_RouteId",
                table: "RouteWorkers",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteWorkers_WorkRoleId",
                table: "RouteWorkers",
                column: "WorkRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteWorkers_WorkerId",
                table: "RouteWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Signins_DepotId",
                table: "Signins",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Signins_WorkerId",
                table: "Signins",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Signins_TenantId_CarryoutDate_DepotId_WorkerId",
                table: "Signins",
                columns: new[] { "TenantId", "CarryoutDate", "DepotId", "WorkerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DepotId",
                table: "Vehicles",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_TenantId_Cn",
                table: "Vehicles",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_DepotId",
                table: "Workers",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_PostId",
                table: "Workers",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_TenantId_Cn",
                table: "Workers",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workplaces_DepotId",
                table: "Workplaces",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Workplaces_TenantId_DepotId_Name",
                table: "Workplaces",
                columns: new[] { "TenantId", "DepotId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkRoles_DefaultPostId",
                table: "WorkRoles",
                column: "DefaultPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_Workers_WorkerId",
                table: "AbpUsers",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ArticleRecords_ArticleRecordId",
                table: "Articles",
                column: "ArticleRecordId",
                principalTable: "ArticleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BoxRecords_Boxes_BoxId",
                table: "BoxRecords",
                column: "BoxId",
                principalTable: "Boxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_Workers_WorkerId",
                table: "AbpUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Workers_CreateWorkerId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteWorkers_Workers_WorkerId",
                table: "RouteWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Depots_DepotId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Depots_DepotId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Depots_DepotId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteWorkers_WorkRoles_WorkRoleId",
                table: "RouteWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleRecords_Articles_ArticleId",
                table: "ArticleRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_BoxRecords_BoxRecordId",
                table: "Boxes");

            migrationBuilder.DropTable(
                name: "AffairEvents");

            migrationBuilder.DropTable(
                name: "AffairTasks");

            migrationBuilder.DropTable(
                name: "AffairWorkers");

            migrationBuilder.DropTable(
                name: "DoorRecords");

            migrationBuilder.DropTable(
                name: "PreRouteTasks");

            migrationBuilder.DropTable(
                name: "PreRouteWorkers");

            migrationBuilder.DropTable(
                name: "RouteArticles");

            migrationBuilder.DropTable(
                name: "RouteEvents");

            migrationBuilder.DropTable(
                name: "RouteInBoxes");

            migrationBuilder.DropTable(
                name: "RouteOutBoxes");

            migrationBuilder.DropTable(
                name: "Signins");

            migrationBuilder.DropTable(
                name: "Affairs");

            migrationBuilder.DropTable(
                name: "PreRoutes");

            migrationBuilder.DropTable(
                name: "RouteTasks");

            migrationBuilder.DropTable(
                name: "Workplaces");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropTable(
                name: "WorkRoles");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "ArticleRecords");

            migrationBuilder.DropTable(
                name: "ArticleTypes");

            migrationBuilder.DropTable(
                name: "RouteWorkers");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "RouteTypes");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "BoxRecords");

            migrationBuilder.DropTable(
                name: "Boxes");

            migrationBuilder.DropTable(
                name: "Outlets");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_WorkerId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "IsWorkerRole",
                table: "AbpRoles");
        }
    }
}
