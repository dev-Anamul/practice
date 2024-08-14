using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateCovids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Covids",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceOfAlert = table.Column<byte>(type: "tinyint", nullable: false),
                    NotificationDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    OtherCovidSymptom = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OtherExposureRisk = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsICUAdmitted = table.Column<bool>(type: "bit", nullable: false),
                    ICUAdmissionDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    IsOnOxygen = table.Column<bool>(type: "bit", nullable: false),
                    OxygenSaturation = table.Column<int>(type: "int", nullable: false),
                    ReceivedBPSupport = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedVentilatorySupport = table.Column<bool>(type: "bit", nullable: false),
                    DateFirstPositive = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    AnyInternationalTravel = table.Column<bool>(type: "bit", nullable: false),
                    TravelDestination = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    IsClientHealthCareWorker = table.Column<bool>(type: "bit", nullable: false),
                    HadCovidExposure = table.Column<bool>(type: "bit", nullable: false),
                    MentalStatusOnAdmission = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HasPneumonia = table.Column<bool>(type: "bit", nullable: false),
                    IsPatientHospitalized = table.Column<bool>(type: "bit", nullable: false),
                    DateHospitalized = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    IsARDS = table.Column<bool>(type: "bit", nullable: false),
                    OtherComorbidConditions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OtherRespiratoryIllness = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_Covids", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Covids_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Covids_ClientId",
                table: "Covids",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Covids");
        }
    }
}
