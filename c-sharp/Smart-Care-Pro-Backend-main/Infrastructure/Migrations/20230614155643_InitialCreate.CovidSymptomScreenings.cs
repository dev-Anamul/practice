using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateCovidSymptomScreenings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CovidSymptomScreenings",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CovidSymptomId = table.Column<int>(type: "int", nullable: false),
                    CovidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_CovidSymptomScreenings", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_CovidSymptomScreenings_Covids_CovidId",
                        column: x => x.CovidId,
                        principalTable: "Covids",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CovidSymptomScreenings_CovidSymptoms_CovidSymptomId",
                        column: x => x.CovidSymptomId,
                        principalTable: "CovidSymptoms",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CovidSymptomScreenings_CovidId",
                table: "CovidSymptomScreenings",
                column: "CovidId");

            migrationBuilder.CreateIndex(
                name: "IX_CovidSymptomScreenings_CovidSymptomId",
                table: "CovidSymptomScreenings",
                column: "CovidSymptomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CovidSymptomScreenings");
        }
    }
}
