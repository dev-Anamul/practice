using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateTakenARTDrugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TakenARTDrugs",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    StoppingReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ARTDrugId = table.Column<int>(type: "int", nullable: false),
                    PriorExposureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriorExposerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_TakenARTDrugs", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_TakenARTDrugs_ARTDrugs_ARTDrugId",
                        column: x => x.ARTDrugId,
                        principalTable: "ARTDrugs",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TakenARTDrugs_PriorARTExposers_PriorExposerId",
                        column: x => x.PriorExposerId,
                        principalTable: "PriorARTExposers",
                        principalColumn: "InteractionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TakenARTDrugs_ARTDrugId",
                table: "TakenARTDrugs",
                column: "ARTDrugId");

            migrationBuilder.CreateIndex(
                name: "IX_TakenARTDrugs_PriorExposerId",
                table: "TakenARTDrugs",
                column: "PriorExposerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TakenARTDrugs");
        }
    }
}
