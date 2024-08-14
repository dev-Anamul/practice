using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateCovaxRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CovaxRecords",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CovaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImmunizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_CovaxRecords", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_CovaxRecords_Covaxes_CovaxId",
                        column: x => x.CovaxId,
                        principalTable: "Covaxes",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CovaxRecords_ImmunizationRecords_ImmunizationId",
                        column: x => x.ImmunizationId,
                        principalTable: "ImmunizationRecords",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CovaxRecords_CovaxId",
                table: "CovaxRecords",
                column: "CovaxId");

            migrationBuilder.CreateIndex(
                name: "IX_CovaxRecords_ImmunizationId",
                table: "CovaxRecords",
                column: "ImmunizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CovaxRecords");
        }
    }
}
