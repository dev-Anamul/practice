using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateSystemRelevances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemsRelevances",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhysicalSystemId = table.Column<int>(type: "int", nullable: false),
                    DrugDefinitionId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SystemsRelevances", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_SystemsRelevances_GeneralDrugDefinitions_DrugDefinitionId",
                        column: x => x.DrugDefinitionId,
                        principalTable: "GeneralDrugDefinitions",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemsRelevances_PhysicalSystems_PhysicalSystemId",
                        column: x => x.PhysicalSystemId,
                        principalTable: "PhysicalSystems",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemsRelevances_DrugDefinitionId",
                table: "SystemsRelevances",
                column: "DrugDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemsRelevances_PhysicalSystemId",
                table: "SystemsRelevances",
                column: "PhysicalSystemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemsRelevances");
        }
    }
}
