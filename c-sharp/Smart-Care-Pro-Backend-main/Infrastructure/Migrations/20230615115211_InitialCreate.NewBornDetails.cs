using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateNewBornDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewBornDetails",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfDelivery = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeOfDelivery = table.Column<TimeSpan>(type: "time", nullable: true),
                    BirthWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BirthHeight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    DeliveredBy = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    Gender = table.Column<byte>(type: "tinyint", nullable: true),
                    PresentingPartId = table.Column<int>(type: "int", nullable: true),
                    BreechId = table.Column<int>(type: "int", nullable: true),
                    ModeOfDeliveryId = table.Column<int>(type: "int", nullable: true),
                    NeonatalBirthOutcomeId = table.Column<int>(type: "int", nullable: true),
                    CauseOfStillbirthId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_NewBornDetails", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_NewBornDetails_Breeches_BreechId",
                        column: x => x.BreechId,
                        principalTable: "Breeches",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_NewBornDetails_CauseOfStillbirths_CauseOfStillbirthId",
                        column: x => x.CauseOfStillbirthId,
                        principalTable: "CauseOfStillbirths",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_NewBornDetails_ModeOfDeliveries_ModeOfDeliveryId",
                        column: x => x.ModeOfDeliveryId,
                        principalTable: "ModeOfDeliveries",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_NewBornDetails_MotherDeliverySummaries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "MotherDeliverySummaries",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewBornDetails_NeonatalBirthOutcomes_NeonatalBirthOutcomeId",
                        column: x => x.NeonatalBirthOutcomeId,
                        principalTable: "NeonatalBirthOutcomes",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_NewBornDetails_PresentingParts_PresentingPartId",
                        column: x => x.PresentingPartId,
                        principalTable: "PresentingParts",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewBornDetails_BreechId",
                table: "NewBornDetails",
                column: "BreechId");

            migrationBuilder.CreateIndex(
                name: "IX_NewBornDetails_CauseOfStillbirthId",
                table: "NewBornDetails",
                column: "CauseOfStillbirthId");

            migrationBuilder.CreateIndex(
                name: "IX_NewBornDetails_DeliveryId",
                table: "NewBornDetails",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_NewBornDetails_ModeOfDeliveryId",
                table: "NewBornDetails",
                column: "ModeOfDeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_NewBornDetails_NeonatalBirthOutcomeId",
                table: "NewBornDetails",
                column: "NeonatalBirthOutcomeId");

            migrationBuilder.CreateIndex(
                name: "IX_NewBornDetails_PresentingPartId",
                table: "NewBornDetails",
                column: "PresentingPartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewBornDetails");
        }
    }
}
