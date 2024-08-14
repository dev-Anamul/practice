using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateMedicineBrands : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicineBrands",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DrugDefinitionId = table.Column<int>(type: "int", nullable: true),
                    SpecialDrugId = table.Column<int>(type: "int", nullable: true),
                    MedicineManufacturerId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_MedicineBrands", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_MedicineBrands_GeneralDrugDefinitions_DrugDefinitionId",
                        column: x => x.DrugDefinitionId,
                        principalTable: "GeneralDrugDefinitions",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_MedicineBrands_MedicineManufacturers_MedicineManufacturerId",
                        column: x => x.MedicineManufacturerId,
                        principalTable: "MedicineManufacturers",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicineBrands_SpecialDrugs_SpecialDrugId",
                        column: x => x.SpecialDrugId,
                        principalTable: "SpecialDrugs",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBrands_DrugDefinitionId",
                table: "MedicineBrands",
                column: "DrugDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBrands_MedicineManufacturerId",
                table: "MedicineBrands",
                column: "MedicineManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBrands_SpecialDrugId",
                table: "MedicineBrands",
                column: "SpecialDrugId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicineBrands");
        }
    }
}
