using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateHTS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HTS",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientSource = table.Column<byte>(type: "tinyint", nullable: false),
                    LastTested = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    LastTestResult = table.Column<byte>(type: "tinyint", nullable: true),
                    PartnerHIVStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    PartnerLastTestDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    HasCounselled = table.Column<byte>(type: "tinyint", nullable: false),
                    HasConsented = table.Column<bool>(type: "bit", nullable: false),
                    NotTestingReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TestNo = table.Column<int>(type: "int", nullable: true),
                    DetermineTestResult = table.Column<byte>(type: "tinyint", nullable: false),
                    BiolineTestResult = table.Column<byte>(type: "tinyint", nullable: true),
                    HIVType = table.Column<byte>(type: "tinyint", nullable: true),
                    IsDNAPCRSampleCollected = table.Column<bool>(type: "bit", nullable: false),
                    SampleCollectionDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    IsResultReceived = table.Column<bool>(type: "bit", nullable: false),
                    RetestDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ConsentForSMS = table.Column<bool>(type: "bit", nullable: false),
                    ClientTypeId = table.Column<int>(type: "int", nullable: false),
                    VisitTypeId = table.Column<int>(type: "int", nullable: false),
                    ServicePointId = table.Column<int>(type: "int", nullable: false),
                    HIVTestingReasonId = table.Column<int>(type: "int", nullable: false),
                    HIVNotTestingReasonId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_HTS", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_HTS_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HTS_ClientTypes_ClientTypeId",
                        column: x => x.ClientTypeId,
                        principalTable: "ClientTypes",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HTS_HIVNotTestingReasons_HIVNotTestingReasonId",
                        column: x => x.HIVNotTestingReasonId,
                        principalTable: "HIVNotTestingReasons",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_HTS_HIVTestingReasons_HIVTestingReasonId",
                        column: x => x.HIVTestingReasonId,
                        principalTable: "HIVTestingReasons",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HTS_ServicePoints_ServicePointId",
                        column: x => x.ServicePointId,
                        principalTable: "ServicePoints",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HTS_VisitTypes_VisitTypeId",
                        column: x => x.VisitTypeId,
                        principalTable: "VisitTypes",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HTS_ClientId",
                table: "HTS",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_HTS_ClientTypeId",
                table: "HTS",
                column: "ClientTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HTS_HIVNotTestingReasonId",
                table: "HTS",
                column: "HIVNotTestingReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_HTS_HIVTestingReasonId",
                table: "HTS",
                column: "HIVTestingReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_HTS_ServicePointId",
                table: "HTS",
                column: "ServicePointId");

            migrationBuilder.CreateIndex(
                name: "IX_HTS_VisitTypeId",
                table: "HTS",
                column: "VisitTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HTS");
        }
    }
}
