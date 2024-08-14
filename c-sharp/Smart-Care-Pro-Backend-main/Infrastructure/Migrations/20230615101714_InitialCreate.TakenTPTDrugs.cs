using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateTakenTPTDrugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TakenTPTDrugs",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TPTDrugId = table.Column<int>(type: "int", nullable: false),
                    TPTHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_TakenTPTDrugs", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_TakenTPTDrugs_TPTDrugs_TPTDrugId",
                        column: x => x.TPTDrugId,
                        principalTable: "TPTDrugs",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TakenTPTDrugs_TPTHistories_TPTHistoryId",
                        column: x => x.TPTHistoryId,
                        principalTable: "TPTHistories",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TakenTPTDrugs_TPTDrugId",
                table: "TakenTPTDrugs",
                column: "TPTDrugId");

            migrationBuilder.CreateIndex(
                name: "IX_TakenTPTDrugs_TPTHistoryId",
                table: "TakenTPTDrugs",
                column: "TPTHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TakenTPTDrugs");
        }
    }
}
