using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateMedications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrescribedDosage = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
                    ItemPerDose = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    TimeUnit = table.Column<byte>(type: "tinyint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    AdditionalDrugTitle = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    AdditionalDrugFormulation = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    PrescribedQuantity = table.Column<int>(type: "int", nullable: false),
                    DispensedDrugTitle = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    DispensedDrugsBrand = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    DispensedDrugsFormulation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DispensedDrugsDosage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DispensedDrugsItemPerDose = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DispensedDrugsFrequency = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DispensedDrugDuration = table.Column<int>(type: "int", nullable: true),
                    DispensedDrugsFrequencyIntervalId = table.Column<int>(type: "int", nullable: true),
                    DispensedDrugsTimeUnit = table.Column<byte>(type: "tinyint", nullable: true),
                    DispensedDrugsRouteId = table.Column<int>(type: "int", nullable: true),
                    DispensedDrugsQuantity = table.Column<int>(type: "int", nullable: true),
                    DispensedDrugsStartDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    DispensedDrugsEndDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ReasonForReplacement = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Frequency = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    GeneralDrugId = table.Column<int>(type: "int", nullable: true),
                    SpecialDrugId = table.Column<int>(type: "int", nullable: true),
                    PrescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FrequencyIntervalId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Medications", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Medications_DrugRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DrugRoutes",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medications_FrequencyIntervals_FrequencyIntervalId",
                        column: x => x.FrequencyIntervalId,
                        principalTable: "FrequencyIntervals",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Medications_GeneralDrugDefinitions_GeneralDrugId",
                        column: x => x.GeneralDrugId,
                        principalTable: "GeneralDrugDefinitions",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Medications_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medications_SpecialDrugs_SpecialDrugId",
                        column: x => x.SpecialDrugId,
                        principalTable: "SpecialDrugs",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medications_FrequencyIntervalId",
                table: "Medications",
                column: "FrequencyIntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_GeneralDrugId",
                table: "Medications",
                column: "GeneralDrugId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_PrescriptionId",
                table: "Medications",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_RouteId",
                table: "Medications",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_SpecialDrugId",
                table: "Medications",
                column: "SpecialDrugId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medications");
        }
    }
}
