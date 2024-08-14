using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateIdentifiedAllergies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentifiedAllergies",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Severity = table.Column<byte>(type: "tinyint", nullable: false),
                    AllergyId = table.Column<int>(type: "int", nullable: false),
                    AllergicDrugId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_IdentifiedAllergies", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_IdentifiedAllergies_AllergicDrugs_AllergicDrugId",
                        column: x => x.AllergicDrugId,
                        principalTable: "AllergicDrugs",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_IdentifiedAllergies_Allergies_AllergyId",
                        column: x => x.AllergyId,
                        principalTable: "Allergies",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentifiedAllergies_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedAllergies_AllergicDrugId",
                table: "IdentifiedAllergies",
                column: "AllergicDrugId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedAllergies_AllergyId",
                table: "IdentifiedAllergies",
                column: "AllergyId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedAllergies_ClientId",
                table: "IdentifiedAllergies",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentifiedAllergies");
        }
    }
}
