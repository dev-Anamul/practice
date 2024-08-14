using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateDiagnoses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diagnoses",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiagnosisType = table.Column<byte>(type: "tinyint", nullable: false),
                    NTGId = table.Column<int>(type: "int", nullable: true),
                    IsSelectedForSurgery = table.Column<bool>(type: "bit", nullable: false),
                    ICDId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurgeryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_Diagnoses", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Diagnoses_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diagnoses_ICDDiagnoses_ICDId",
                        column: x => x.ICDId,
                        principalTable: "ICDDiagnoses",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Diagnoses_NTGLevelThreeDiagnoses_NTGId",
                        column: x => x.NTGId,
                        principalTable: "NTGLevelThreeDiagnoses",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Diagnoses_Surgeries_SurgeryId",
                        column: x => x.SurgeryId,
                        principalTable: "Surgeries",
                        principalColumn: "InteractionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_ClientId",
                table: "Diagnoses",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_ICDId",
                table: "Diagnoses",
                column: "ICDId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_NTGId",
                table: "Diagnoses",
                column: "NTGId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnoses_SurgeryId",
                table: "Diagnoses",
                column: "SurgeryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diagnoses");
        }
    }
}
