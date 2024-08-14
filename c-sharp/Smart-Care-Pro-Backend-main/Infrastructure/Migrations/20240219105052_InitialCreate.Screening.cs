using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateScreening : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Screenings",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPapSmearDone = table.Column<bool>(type: "bit", nullable: false),
                    PapSmearTestResult = table.Column<byte>(type: "tinyint", nullable: true),
                    PapSmearGrade = table.Column<byte>(type: "tinyint", nullable: true),
                    PapSmearComment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HPVTestType = table.Column<byte>(type: "tinyint", nullable: true),
                    HPVTestResult = table.Column<byte>(type: "tinyint", nullable: true),
                    HPVGenoTypeFound = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    HPVGenoTypeComment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsVIADone = table.Column<bool>(type: "bit", nullable: true),
                    VIAWhyNotDone = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsVIATransformationZoneSeen = table.Column<bool>(type: "bit", nullable: true),
                    VIAStateType = table.Column<byte>(type: "tinyint", nullable: true),
                    VIAIsLesionSeen = table.Column<bool>(type: "bit", nullable: true),
                    VIAIsAtypicalVessels = table.Column<bool>(type: "bit", nullable: true),
                    VIAIsLesionExtendedIntoCervicalOs = table.Column<bool>(type: "bit", nullable: true),
                    VIAIsMosiacism = table.Column<bool>(type: "bit", nullable: true),
                    VIAIsLesionCovers = table.Column<bool>(type: "bit", nullable: true),
                    VIAIsQueryICC = table.Column<bool>(type: "bit", nullable: true),
                    VIAIsPunctations = table.Column<bool>(type: "bit", nullable: true),
                    VIAIsEctopy = table.Column<bool>(type: "bit", nullable: true),
                    VIATestResult = table.Column<byte>(type: "tinyint", nullable: true),
                    VIAComments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsEDIDone = table.Column<bool>(type: "bit", nullable: true),
                    EDIWhyNotDone = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsEDITransformationZoneSeen = table.Column<bool>(type: "bit", nullable: true),
                    EDIStateType = table.Column<byte>(type: "tinyint", nullable: true),
                    EDIIsLesionSeen = table.Column<bool>(type: "bit", nullable: true),
                    EDIIsAtypicalVessels = table.Column<bool>(type: "bit", nullable: true),
                    EDIIsLesionExtendedIntoCervicalOs = table.Column<bool>(type: "bit", nullable: true),
                    EDIIsMosiacism = table.Column<bool>(type: "bit", nullable: true),
                    EDIIsLesionCoversMoreThreeQuaters = table.Column<bool>(type: "bit", nullable: true),
                    EDIIsQueryICC = table.Column<bool>(type: "bit", nullable: true),
                    EDIIsPunctations = table.Column<bool>(type: "bit", nullable: true),
                    EDIIsEctopy = table.Column<bool>(type: "bit", nullable: true),
                    EDITestResult = table.Column<byte>(type: "tinyint", nullable: true),
                    EDIComments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_Screenings", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Screenings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screenings_ClientId",
                table: "Screenings",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Screenings");
        }
    }
}
