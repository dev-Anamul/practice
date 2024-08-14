using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateCACXPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaCXPlans",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsClientReffered = table.Column<bool>(type: "bit", nullable: false),
                    IsLesionExtendsIntoTheCervicalOs = table.Column<bool>(type: "bit", nullable: false),
                    IsQueryICC = table.Column<bool>(type: "bit", nullable: false),
                    IsAtypicalVessels = table.Column<bool>(type: "bit", nullable: false),
                    IsPunctationsOrMoasicm = table.Column<bool>(type: "bit", nullable: false),
                    IsLesionCovers = table.Column<bool>(type: "bit", nullable: false),
                    IsPoly = table.Column<bool>(type: "bit", nullable: false),
                    IsLesionTooLargeForThermoAblation = table.Column<bool>(type: "bit", nullable: false),
                    IsLesionToThickForAblation = table.Column<bool>(type: "bit", nullable: false),
                    IsPostLEEPLesion = table.Column<bool>(type: "bit", nullable: false),
                    Others = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterType = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedIn = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedIn = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaCXPlans", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_CaCXPlans_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterCourseStatuses",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
                    CreatedIn = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedIn = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterCourseStatuses", x => x.Oid);
                });

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
                name: "IX_CaCXPlans_ClientId",
                table: "CaCXPlans",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_GynObsHistoryInterCourseStatus_InterCourseStatusesOid",
                table: "GynObsHistoryInterCourseStatus",
                column: "InterCourseStatusesOid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaCXPlans");

            migrationBuilder.DropTable(
                name: "GynObsHistoryInterCourseStatus");

            migrationBuilder.DropTable(
                name: "InterCourseStatuses");
        }
    }
}
