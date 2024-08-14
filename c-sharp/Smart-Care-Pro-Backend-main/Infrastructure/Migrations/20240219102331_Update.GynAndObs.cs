using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateGynAndObs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GynObsHistoryInterCourseStatus");

            migrationBuilder.AddColumn<byte>(
                name: "AgeAtMenarche",
                table: "GynObsHistories",
                type: "Tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Examination",
                table: "GynObsHistories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "FirstPregnancyAge",
                table: "GynObsHistories",
                type: "Tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "FirstSexualIntercourseAge",
                table: "GynObsHistories",
                type: "Tinyint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAbnormalVaginalDischarge",
                table: "GynObsHistories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasFever",
                table: "GynObsHistories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasLowerAbdominalPain",
                table: "GynObsHistories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntercourseStatusId",
                table: "GynObsHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAnythingUsedToCleanVagina",
                table: "GynObsHistories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBleedingDuringOrAfterCoitus",
                table: "GynObsHistories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMensAssociatedWithPain",
                table: "GynObsHistories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemUsedToCleanVagina",
                table: "GynObsHistories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MenstrualBloodFlow",
                table: "GynObsHistories",
                type: "Tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MenstrualCycleRegularity",
                table: "GynObsHistories",
                type: "Tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "NumberOfSexualPartners",
                table: "GynObsHistories",
                type: "Tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherConcern",
                table: "GynObsHistories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "UsedFor",
                table: "ContraceptiveHistories",
                type: "Tinyint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GynObsHistories_IntercourseStatusId",
                table: "GynObsHistories",
                column: "IntercourseStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_GynObsHistories_InterCourseStatuses_IntercourseStatusId",
                table: "GynObsHistories",
                column: "IntercourseStatusId",
                principalTable: "InterCourseStatuses",
                principalColumn: "Oid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GynObsHistories_InterCourseStatuses_IntercourseStatusId",
                table: "GynObsHistories");

            migrationBuilder.DropIndex(
                name: "IX_GynObsHistories_IntercourseStatusId",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "AgeAtMenarche",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "Examination",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "FirstPregnancyAge",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "FirstSexualIntercourseAge",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "HasAbnormalVaginalDischarge",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "HasFever",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "HasLowerAbdominalPain",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "IntercourseStatusId",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "IsAnythingUsedToCleanVagina",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "IsBleedingDuringOrAfterCoitus",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "IsMensAssociatedWithPain",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "ItemUsedToCleanVagina",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "MenstrualBloodFlow",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "MenstrualCycleRegularity",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "NumberOfSexualPartners",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "OtherConcern",
                table: "GynObsHistories");

            migrationBuilder.DropColumn(
                name: "UsedFor",
                table: "ContraceptiveHistories");

            migrationBuilder.CreateTable(
                name: "GynObsHistoryInterCourseStatus",
                columns: table => new
                {
                    GynObsHistoryInteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterCourseStatusesOid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GynObsHistoryInterCourseStatus", x => new { x.GynObsHistoryInteractionId, x.InterCourseStatusesOid });
                    table.ForeignKey(
                        name: "FK_GynObsHistoryInterCourseStatus_GynObsHistories_GynObsHistoryInteractionId",
                        column: x => x.GynObsHistoryInteractionId,
                        principalTable: "GynObsHistories",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GynObsHistoryInterCourseStatus_InterCourseStatuses_InterCourseStatusesOid",
                        column: x => x.InterCourseStatusesOid,
                        principalTable: "InterCourseStatuses",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GynObsHistoryInterCourseStatus_InterCourseStatusesOid",
                table: "GynObsHistoryInterCourseStatus",
                column: "InterCourseStatusesOid");
        }
    }
}
