using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateDispensedItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DispensedItems",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReasonForReplacement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedDosage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ItemPerDose = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Frequency = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FrequencyIntervalId = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    TimeUnit = table.Column<byte>(type: "tinyint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    NumberOfUnitDispensed = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DispenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneralMedicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MedicineBrandId = table.Column<int>(type: "int", nullable: false),
                    DrugDefinitionId = table.Column<int>(type: "int", nullable: true),
                    RouteId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_DispensedItems", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_DispensedItems_Dispenses_DispenseId",
                        column: x => x.DispenseId,
                        principalTable: "Dispenses",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispensedItems_DrugRoutes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DrugRoutes",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_DispensedItems_GeneralDrugDefinitions_DrugDefinitionId",
                        column: x => x.DrugDefinitionId,
                        principalTable: "GeneralDrugDefinitions",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_DispensedItems_Medications_GeneralMedicationId",
                        column: x => x.GeneralMedicationId,
                        principalTable: "Medications",
                        principalColumn: "InteractionId");
                    table.ForeignKey(
                        name: "FK_DispensedItems_MedicineBrands_MedicineBrandId",
                        column: x => x.MedicineBrandId,
                        principalTable: "MedicineBrands",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispensedItems_DispenseId",
                table: "DispensedItems",
                column: "DispenseId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensedItems_DrugDefinitionId",
                table: "DispensedItems",
                column: "DrugDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensedItems_GeneralMedicationId",
                table: "DispensedItems",
                column: "GeneralMedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensedItems_MedicineBrandId",
                table: "DispensedItems",
                column: "MedicineBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensedItems_RouteId",
                table: "DispensedItems",
                column: "RouteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispensedItems");
        }
    }
}
