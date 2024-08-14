using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateIdentifiedConstitutionalSymptoms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentifiedConstitutionalSymptoms",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConstitutionalSymptomTypeId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_IdentifiedConstitutionalSymptoms", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_IdentifiedConstitutionalSymptoms_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentifiedConstitutionalSymptoms_ConstitutionalSymptomTypes_ConstitutionalSymptomTypeId",
                        column: x => x.ConstitutionalSymptomTypeId,
                        principalTable: "ConstitutionalSymptomTypes",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedConstitutionalSymptoms_ClientId",
                table: "IdentifiedConstitutionalSymptoms",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedConstitutionalSymptoms_ConstitutionalSymptomTypeId",
                table: "IdentifiedConstitutionalSymptoms",
                column: "ConstitutionalSymptomTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentifiedConstitutionalSymptoms");
        }
    }
}
