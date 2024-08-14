using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateARTDrugAdherences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ARTDrugAdherences",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HaveTroubleTakingPills = table.Column<bool>(type: "bit", nullable: false),
                    HowManyDosesMissed = table.Column<byte>(type: "tinyint", nullable: false),
                    ReducePharmacyVisitTo = table.Column<byte>(type: "tinyint", nullable: true),
                    ReferForAdherenceCounselling = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Forgot = table.Column<bool>(type: "bit", nullable: false),
                    Illness = table.Column<bool>(type: "bit", nullable: false),
                    SideEffect = table.Column<bool>(type: "bit", nullable: false),
                    MedicineFinished = table.Column<bool>(type: "bit", nullable: false),
                    AwayFromHome = table.Column<bool>(type: "bit", nullable: false),
                    ClinicRunOutOfMedication = table.Column<bool>(type: "bit", nullable: false),
                    OtherMissingReason = table.Column<bool>(type: "bit", nullable: false),
                    Nausea = table.Column<bool>(type: "bit", nullable: false),
                    Vomitting = table.Column<bool>(type: "bit", nullable: false),
                    YellowEyes = table.Column<bool>(type: "bit", nullable: false),
                    MouthSores = table.Column<bool>(type: "bit", nullable: false),
                    Diarrhea = table.Column<bool>(type: "bit", nullable: false),
                    Headache = table.Column<bool>(type: "bit", nullable: false),
                    Rash = table.Column<bool>(type: "bit", nullable: false),
                    Numbness = table.Column<bool>(type: "bit", nullable: false),
                    Insomnia = table.Column<bool>(type: "bit", nullable: false),
                    Depression = table.Column<bool>(type: "bit", nullable: false),
                    WeightGain = table.Column<bool>(type: "bit", nullable: false),
                    OtherSideEffect = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrugId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ARTDrugAdherences", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ARTDrugAdherences_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ARTDrugAdherences_SpecialDrugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "SpecialDrugs",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ARTDrugAdherences_ClientId",
                table: "ARTDrugAdherences",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ARTDrugAdherences_DrugId",
                table: "ARTDrugAdherences",
                column: "DrugId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ARTDrugAdherences");
        }
    }
}
