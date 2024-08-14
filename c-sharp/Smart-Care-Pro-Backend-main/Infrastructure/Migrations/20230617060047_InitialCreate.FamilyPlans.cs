using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateFamilyPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FamilyPlans",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnySexualViolenceSymptoms = table.Column<byte>(type: "tinyint", nullable: true),
                    ClientIsNotPregnant = table.Column<byte>(type: "tinyint", nullable: true),
                    ReasonOfNotPregnant = table.Column<byte>(type: "tinyint", nullable: true),
                    HasConsentForFP = table.Column<byte>(type: "tinyint", nullable: true),
                    FPMethodPlan = table.Column<byte>(type: "tinyint", nullable: true),
                    FPMethodPlanRequest = table.Column<byte>(type: "tinyint", nullable: true),
                    ClientPreferences = table.Column<byte>(type: "tinyint", nullable: true),
                    SelectedFamilyPlan = table.Column<byte>(type: "tinyint", nullable: true),
                    ClassifyFPMethod = table.Column<byte>(type: "tinyint", nullable: true),
                    FamilyPlans = table.Column<byte>(type: "tinyint", nullable: true),
                    FPProvidedPlace = table.Column<byte>(type: "tinyint", nullable: true),
                    ReasonForNoPlan = table.Column<byte>(type: "tinyint", nullable: true),
                    ClientReceivePreferredOptions = table.Column<byte>(type: "tinyint", nullable: true),
                    ClientNotReceivePreferredOptions = table.Column<byte>(type: "tinyint", nullable: true),
                    BackupMethodUsed = table.Column<byte>(type: "tinyint", nullable: true),
                    IsHIVTestingNeed = table.Column<bool>(type: "bit", nullable: false),
                    IsSTI = table.Column<bool>(type: "bit", nullable: false),
                    IsCervicalCancer = table.Column<bool>(type: "bit", nullable: false),
                    IsBreastCancer = table.Column<bool>(type: "bit", nullable: false),
                    IsProstateCancer = table.Column<bool>(type: "bit", nullable: false),
                    SubclassId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FamilyPlans", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_FamilyPlans_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyPlans_FamilyPlanningSubclasses_SubclassId",
                        column: x => x.SubclassId,
                        principalTable: "FamilyPlanningSubclasses",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyPlans_ClientId",
                table: "FamilyPlans",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyPlans_SubclassId",
                table: "FamilyPlans",
                column: "SubclassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyPlans");
        }
    }
}
