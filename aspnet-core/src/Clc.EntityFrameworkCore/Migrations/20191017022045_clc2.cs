using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Clc.Migrations
{
    public partial class clc2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DoorIp",
                table: "Workplaces",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CameraIp",
                table: "Workplaces",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Finger",
                table: "Workers",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArticleTypeBinds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    DepotId = table.Column<int>(nullable: false),
                    ArticleTypeId = table.Column<int>(nullable: false),
                    BindStyle = table.Column<string>(maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTypeBinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTypeBinds_ArticleTypes_ArticleTypeId",
                        column: x => x.ArticleTypeId,
                        principalTable: "ArticleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTypeBinds_Depots_DepotId",
                        column: x => x.DepotId,
                        principalTable: "Depots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false),
                    FileCn = table.Column<string>(maxLength: 20, nullable: false),
                    Hiredate = table.Column<DateTime>(nullable: false),
                    Sex = table.Column<string>(maxLength: 4, nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    EndValidity = table.Column<DateTime>(nullable: false),
                    Ethnicity = table.Column<string>(maxLength: 8, nullable: true),
                    Nativeplace = table.Column<string>(maxLength: 10, nullable: true),
                    ResidenceAddress = table.Column<string>(maxLength: 50, nullable: true),
                    PoliceStation = table.Column<string>(maxLength: 4, nullable: true),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    PoliticalStatus = table.Column<string>(maxLength: 4, nullable: false),
                    Education = table.Column<string>(maxLength: 4, nullable: false),
                    School = table.Column<string>(maxLength: 50, nullable: true),
                    Stature = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    MaritalStatus = table.Column<string>(maxLength: 4, nullable: true),
                    Contact = table.Column<string>(maxLength: 50, nullable: true),
                    LicenseType = table.Column<string>(maxLength: 4, nullable: true),
                    Introductory = table.Column<string>(maxLength: 50, nullable: true),
                    Insurance = table.Column<string>(maxLength: 50, nullable: true),
                    WorkLicenseCn = table.Column<string>(maxLength: 50, nullable: true),
                    CertificateCn = table.Column<string>(maxLength: 50, nullable: true),
                    ArmLicenceCn = table.Column<string>(maxLength: 50, nullable: true),
                    CeteficationRecord = table.Column<string>(nullable: true),
                    JobChangeRecord = table.Column<string>(nullable: true),
                    MobilityRecord = table.Column<string>(nullable: true),
                    TrainingRecord = table.Column<string>(nullable: true),
                    RewardPunishRecord = table.Column<string>(nullable: true),
                    Status = table.Column<string>(maxLength: 4, nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    QuitFileCn = table.Column<string>(maxLength: 20, nullable: true),
                    Quitdate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerFiles_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypeBinds_ArticleTypeId",
                table: "ArticleTypeBinds",
                column: "ArticleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypeBinds_DepotId",
                table: "ArticleTypeBinds",
                column: "DepotId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTypeBinds_TenantId_ArticleTypeId",
                table: "ArticleTypeBinds",
                columns: new[] { "TenantId", "ArticleTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerFiles_WorkerId",
                table: "WorkerFiles",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerFiles_TenantId_WorkerId",
                table: "WorkerFiles",
                columns: new[] { "TenantId", "WorkerId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTypeBinds");

            migrationBuilder.DropTable(
                name: "WorkerFiles");

            migrationBuilder.DropColumn(
                name: "Finger",
                table: "Workers");

            migrationBuilder.AlterColumn<string>(
                name: "DoorIp",
                table: "Workplaces",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CameraIp",
                table: "Workplaces",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
