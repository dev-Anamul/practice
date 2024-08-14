using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateGynObsHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GynObsHistories",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenstrualHistory = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsBreastFeeding = table.Column<byte>(type: "tinyint", nullable: true),
                    LNMP = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    IsPregnant = table.Column<byte>(type: "tinyint", nullable: true),
                    ObstetricsHistoryNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GestationalAge = table.Column<int>(type: "int", nullable: true),
                    EDD = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    IsCaCxScreened = table.Column<byte>(type: "tinyint", nullable: true),
                    CaCxLastScreened = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CaCxResult = table.Column<byte>(type: "tinyint", nullable: true),
                    IsChildTestedForHIV = table.Column<byte>(type: "tinyint", nullable: true),
                    IsScreenedForSyphilis = table.Column<byte>(type: "tinyint", nullable: true),
                    HaveTreatedWithBenzathinePenicillin = table.Column<byte>(type: "tinyint", nullable: true),
                    BreastFeedingChoice = table.Column<byte>(type: "tinyint", nullable: true),
                    BreastFeedingType = table.Column<byte>(type: "tinyint", nullable: true),
                    BreastFeedingNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCientOnFP = table.Column<bool>(type: "bit", nullable: false),
                    IsClientNeedFP = table.Column<bool>(type: "bit", nullable: false),
                    CurrentFP = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    HasCounselled = table.Column<bool>(type: "bit", nullable: false),
                    ContraceptiveGiven = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PreviousSexualHistory = table.Column<bool>(type: "bit", nullable: false),
                    PreviouslyGotPregnant = table.Column<byte>(type: "tinyint", nullable: true),
                    TotalNumberOfPregnancy = table.Column<int>(type: "int", nullable: true),
                    TotalBirthGiven = table.Column<int>(type: "int", nullable: true),
                    AliveChildren = table.Column<int>(type: "int", nullable: true),
                    CesareanHistory = table.Column<byte>(type: "tinyint", nullable: true),
                    RecentClientGivenBirth = table.Column<byte>(type: "tinyint", nullable: true),
                    DateOfDelivery = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Postpartum = table.Column<byte>(type: "tinyint", nullable: true),
                    LastDeliveryTime = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    MiscarriageStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    MiscarriageWithinFourWeeks = table.Column<byte>(type: "tinyint", nullable: true),
                    PostAbortionSepsis = table.Column<byte>(type: "tinyint", nullable: true),
                    CaCxScreeningMethodId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_GynObsHistories", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_GynObsHistories_CaCxScreeningMethods_CaCxScreeningMethodId",
                        column: x => x.CaCxScreeningMethodId,
                        principalTable: "CaCxScreeningMethods",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_GynObsHistories_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GynObsHistories_CaCxScreeningMethodId",
                table: "GynObsHistories",
                column: "CaCxScreeningMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_GynObsHistories_ClientId",
                table: "GynObsHistories",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GynObsHistories");
        }
    }
}
