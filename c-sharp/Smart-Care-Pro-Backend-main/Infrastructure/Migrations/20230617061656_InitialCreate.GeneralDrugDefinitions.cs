using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateGeneralDrugDefinitions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralDrugDefinitions",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Strength = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DosageUnitId = table.Column<int>(type: "int", nullable: false),
                    FormulationId = table.Column<int>(type: "int", nullable: false),
                    DrugUtilityId = table.Column<int>(type: "int", nullable: false),
                    GenericDrugId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_GeneralDrugDefinitions", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_GeneralDrugDefinitions_DrugDosageUnites_DosageUnitId",
                        column: x => x.DosageUnitId,
                        principalTable: "DrugDosageUnites",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralDrugDefinitions_DrugFormulations_FormulationId",
                        column: x => x.FormulationId,
                        principalTable: "DrugFormulations",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralDrugDefinitions_DrugUtilities_DrugUtilityId",
                        column: x => x.DrugUtilityId,
                        principalTable: "DrugUtilities",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralDrugDefinitions_GenericDrugs_GenericDrugId",
                        column: x => x.GenericDrugId,
                        principalTable: "GenericDrugs",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralDrugDefinitions_DosageUnitId",
                table: "GeneralDrugDefinitions",
                column: "DosageUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralDrugDefinitions_DrugUtilityId",
                table: "GeneralDrugDefinitions",
                column: "DrugUtilityId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralDrugDefinitions_FormulationId",
                table: "GeneralDrugDefinitions",
                column: "FormulationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralDrugDefinitions_GenericDrugId",
                table: "GeneralDrugDefinitions",
                column: "GenericDrugId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralDrugDefinitions");
        }
    }
}
