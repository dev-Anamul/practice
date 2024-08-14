using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreatePlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Plans = table.Column<byte>(type: "tinyint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    StopDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsUrinalysisNormal = table.Column<bool>(type: "bit", nullable: false),
                    HasAcuteHIVInfectionSymptoms = table.Column<bool>(type: "bit", nullable: false),
                    IsAbleToAdhereDailyPrEP = table.Column<bool>(type: "bit", nullable: false),
                    HasGreaterFiftyCreatinineClearance = table.Column<bool>(type: "bit", nullable: false),
                    IsPotentialHIVExposureMoreThanSixWeeksOld = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoppingReasonId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Plans", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Plans_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plans_StoppingReasons_StoppingReasonId",
                        column: x => x.StoppingReasonId,
                        principalTable: "StoppingReasons",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plans_ClientId",
                table: "Plans",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_StoppingReasonId",
                table: "Plans",
                column: "StoppingReasonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
