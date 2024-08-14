using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateSpecialDrugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialDrugs",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    DosageUnitId = table.Column<int>(type: "int", nullable: true),
                    FormulationId = table.Column<int>(type: "int", nullable: true),
                    RegimenId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SpecialDrugs", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_SpecialDrugs_DrugDosageUnites_DosageUnitId",
                        column: x => x.DosageUnitId,
                        principalTable: "DrugDosageUnites",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_SpecialDrugs_DrugFormulations_FormulationId",
                        column: x => x.FormulationId,
                        principalTable: "DrugFormulations",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_SpecialDrugs_DrugRegimens_RegimenId",
                        column: x => x.RegimenId,
                        principalTable: "DrugRegimens",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDrugs_DosageUnitId",
                table: "SpecialDrugs",
                column: "DosageUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDrugs_FormulationId",
                table: "SpecialDrugs",
                column: "FormulationId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDrugs_RegimenId",
                table: "SpecialDrugs",
                column: "RegimenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialDrugs");
        }
    }
}
