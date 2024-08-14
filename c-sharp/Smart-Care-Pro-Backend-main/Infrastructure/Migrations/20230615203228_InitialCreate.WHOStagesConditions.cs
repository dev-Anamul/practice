using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateWHOStagesConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WHOStagesConditions",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
                    ForAdolescent = table.Column<bool>(type: "bit", nullable: false),
                    ForAdult = table.Column<bool>(type: "bit", nullable: false),
                    ForPregnant = table.Column<bool>(type: "bit", nullable: false),
                    WHOClinicalStageId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_WHOStagesConditions", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_WHOStagesConditions_WHOClinicalStages_WHOClinicalStageId",
                        column: x => x.WHOClinicalStageId,
                        principalTable: "WHOClinicalStages",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WHOStagesConditions_WHOClinicalStageId",
                table: "WHOStagesConditions",
                column: "WHOClinicalStageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WHOStagesConditions");
        }
    }
}
