using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateMedicalConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalConditions",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoesAnyHealthConditonScreened = table.Column<byte>(type: "tinyint", nullable: true),
                    DoesRiskOfSTIIncreased = table.Column<byte>(type: "tinyint", nullable: true),
                    LastUnprotectedSexDays = table.Column<int>(type: "int", nullable: true),
                    PastMedicalConditonId = table.Column<int>(type: "int", nullable: true),
                    STIRiskId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_MedicalConditions", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_MedicalConditions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalConditions_PastMedicalConditons_PastMedicalConditonId",
                        column: x => x.PastMedicalConditonId,
                        principalTable: "PastMedicalConditons",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_MedicalConditions_STIRisks_STIRiskId",
                        column: x => x.STIRiskId,
                        principalTable: "STIRisks",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConditions_ClientId",
                table: "MedicalConditions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConditions_PastMedicalConditonId",
                table: "MedicalConditions",
                column: "PastMedicalConditonId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConditions_STIRiskId",
                table: "MedicalConditions",
                column: "STIRiskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalConditions");
        }
    }
}
