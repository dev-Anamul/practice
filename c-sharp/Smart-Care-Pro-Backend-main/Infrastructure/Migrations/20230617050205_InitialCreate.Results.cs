using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResultDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    ResultDescriptive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultNumeric = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CommentOnResult = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsResultNormal = table.Column<byte>(type: "tinyint", nullable: false),
                    MeasuringUnitId = table.Column<int>(type: "int", nullable: true),
                    ResultOptionId = table.Column<int>(type: "int", nullable: true),
                    InvestigationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Results", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Results_Investigations_InvestigationId",
                        column: x => x.InvestigationId,
                        principalTable: "Investigations",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Results_MeasuringUnits_MeasuringUnitId",
                        column: x => x.MeasuringUnitId,
                        principalTable: "MeasuringUnits",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Results_ResultOptions_ResultOptionId",
                        column: x => x.ResultOptionId,
                        principalTable: "ResultOptions",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_InvestigationId",
                table: "Results",
                column: "InvestigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_MeasuringUnitId",
                table: "Results",
                column: "MeasuringUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_ResultOptionId",
                table: "Results",
                column: "ResultOptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");
        }
    }
}
