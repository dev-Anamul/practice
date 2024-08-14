using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateThirdStageDeliveries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThirdStageDeliveries",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActiveManagement = table.Column<bool>(type: "bit", nullable: false),
                    PPH = table.Column<bool>(type: "bit", nullable: false),
                    BloodLoss = table.Column<int>(type: "int", nullable: true),
                    IsUterineAtony = table.Column<bool>(type: "bit", nullable: false),
                    IsUterineInversion = table.Column<bool>(type: "bit", nullable: false),
                    IsAbnormalPlacemental = table.Column<bool>(type: "bit", nullable: false),
                    IsBirthTrauma = table.Column<bool>(type: "bit", nullable: false),
                    IsCoagulationDisorder = table.Column<bool>(type: "bit", nullable: false),
                    Others = table.Column<bool>(type: "bit", nullable: false),
                    IsMultiplePregnancy = table.Column<bool>(type: "bit", nullable: false),
                    IsProlongedOxytocinUse = table.Column<bool>(type: "bit", nullable: false),
                    IsUterineLeiomyoma = table.Column<bool>(type: "bit", nullable: false),
                    IsAnesthesia = table.Column<bool>(type: "bit", nullable: false),
                    IsLatrogenicInjury = table.Column<bool>(type: "bit", nullable: false),
                    IsUterineRapture = table.Column<bool>(type: "bit", nullable: false),
                    IsFetalMacrosomia = table.Column<bool>(type: "bit", nullable: false),
                    IsMalpresentationOfFetus = table.Column<bool>(type: "bit", nullable: false),
                    IsRetainedPlacenta = table.Column<bool>(type: "bit", nullable: false),
                    IsAbnormalPlacentation = table.Column<bool>(type: "bit", nullable: false),
                    IsUncontrolledCordContraction = table.Column<bool>(type: "bit", nullable: false),
                    IsPreviousUterineInversion = table.Column<bool>(type: "bit", nullable: false),
                    IsVonWillebrand = table.Column<bool>(type: "bit", nullable: false),
                    IsHemophilia = table.Column<bool>(type: "bit", nullable: false),
                    IsVelamentousCordInsertion = table.Column<bool>(type: "bit", nullable: false),
                    IsRetainedProductOfConception = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ThirdStageDeliveries", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ThirdStageDeliveries_MotherDeliverySummaries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "MotherDeliverySummaries",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThirdStageDeliveries_DeliveryId",
                table: "ThirdStageDeliveries",
                column: "DeliveryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThirdStageDeliveries");
        }
    }
}
