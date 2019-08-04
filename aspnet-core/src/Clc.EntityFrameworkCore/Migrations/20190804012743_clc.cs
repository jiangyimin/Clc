using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clc.Migrations
{
    public partial class clc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "AffairTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 1, nullable: false),
                    Name = table.Column<string>(maxLength: 8, nullable: false),
                    hasCloudDoor = table.Column<bool>(nullable: false),
                    toRoute = table.Column<bool>(nullable: false),
                    MinDuration = table.Column<int>(nullable: false),
                    MaxDuration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffairTypes", x => x.Id);
                });

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
                    UnlockScreenPassword = table.Column<string>(maxLength: 8, nullable: true)
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
                    BindInfo = table.Column<string>(maxLength: 20, nullable: true)
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
                    AffairTypeId = table.Column<int>(nullable: false),
                    ArticleTypeList = table.Column<string>(maxLength: 50, nullable: true),
                    ShareDepotList = table.Column<string>(maxLength: 50, nullable: true),
                    RoleUserName = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workplaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workplaces_AffairTypes_AffairTypeId",
                        column: x => x.AffairTypeId,
                        principalTable: "AffairTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Rfid = table.Column<string>(maxLength: 18, nullable: true),
                    Photo = table.Column<byte[]>(nullable: true),
                    DeviceId = table.Column<string>(maxLength: 50, nullable: true),
                    AdditiveInfo = table.Column<string>(maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LoanDepotId = table.Column<int>(nullable: true)
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
                        name: "FK_Workers_Depots_LoanDepotId",
                        column: x => x.LoanDepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Boxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    OutletId = table.Column<int>(nullable: false),
                    Cn = table.Column<string>(maxLength: 10, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Contact = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boxes_Outlets_OutletId",
                        column: x => x.OutletId,
                        principalTable: "Outlets",
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
                    isTomorrow = table.Column<bool>(nullable: false),
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
                name: "Signins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    SigninTime = table.Column<DateTime>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false)
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
                name: "AffairEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    AffairId = table.Column<int>(nullable: false),
                    EventTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: true),
                    Content = table.Column<string>(maxLength: 50, nullable: true),
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
                name: "AffairTask",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    AffairId = table.Column<int>(nullable: false),
                    WorkplaceId = table.Column<int>(nullable: false),
                    StartTime = table.Column<string>(maxLength: 5, nullable: false),
                    EndTime = table.Column<string>(maxLength: 5, nullable: false),
                    isTomorrow = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 50, nullable: true),
                    CreateWorkerId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffairTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffairTask_Affairs_AffairId",
                        column: x => x.AffairId,
                        principalTable: "Affairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AffairTask_Workers_CreateWorkerId",
                        column: x => x.CreateWorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AffairTask_Workplaces_WorkplaceId",
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
                name: "IX_AffairTask_AffairId",
                table: "AffairTask",
                column: "AffairId");

            migrationBuilder.CreateIndex(
                name: "IX_AffairTask_CreateWorkerId",
                table: "AffairTask",
                column: "CreateWorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_AffairTask_WorkplaceId",
                table: "AffairTask",
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
                name: "IX_Boxes_OutletId",
                table: "Boxes",
                column: "OutletId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_TenantId_Cn",
                table: "Boxes",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

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
                name: "IX_Outlets_CustomerId",
                table: "Outlets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_TenantId_Cn",
                table: "Outlets",
                columns: new[] { "TenantId", "Cn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Signins_DepotId",
                table: "Signins",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_Signins_WorkerId",
                table: "Signins",
                column: "WorkerId");

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
                name: "IX_Workers_LoanDepotId",
                table: "Workers",
                column: "LoanDepotId");

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
                name: "IX_Workplaces_AffairTypeId",
                table: "Workplaces",
                column: "AffairTypeId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AffairEvents");

            migrationBuilder.DropTable(
                name: "AffairTask");

            migrationBuilder.DropTable(
                name: "AffairWorkers");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Boxes");

            migrationBuilder.DropTable(
                name: "RouteTypes");

            migrationBuilder.DropTable(
                name: "Signins");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Affairs");

            migrationBuilder.DropTable(
                name: "WorkRoles");

            migrationBuilder.DropTable(
                name: "ArticleTypes");

            migrationBuilder.DropTable(
                name: "Outlets");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Workplaces");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "AffairTypes");

            migrationBuilder.DropTable(
                name: "Depots");

            migrationBuilder.DropColumn(
                name: "TwinUserName",
                table: "AbpRoles");

            migrationBuilder.DropColumn(
                name: "TwinUserPassword",
                table: "AbpRoles");
        }
    }
}
