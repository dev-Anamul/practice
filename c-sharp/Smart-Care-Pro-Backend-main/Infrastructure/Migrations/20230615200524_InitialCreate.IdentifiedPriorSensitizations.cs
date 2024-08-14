using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateIdentifiedPriorSensitizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentifiedPriorSensitizations",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BloodTransfusionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriorSensitizationId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_IdentifiedPriorSensitizations", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_IdentifiedPriorSensitizations_BloodTransfusionHistories_BloodTransfusionId",
                        column: x => x.BloodTransfusionId,
                        principalTable: "BloodTransfusionHistories",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentifiedPriorSensitizations_PriorSensitizations_PriorSensitizationId",
                        column: x => x.PriorSensitizationId,
                        principalTable: "PriorSensitizations",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedPriorSensitizations_BloodTransfusionId",
                table: "IdentifiedPriorSensitizations",
                column: "BloodTransfusionId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedPriorSensitizations_PriorSensitizationId",
                table: "IdentifiedPriorSensitizations",
                column: "PriorSensitizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentifiedPriorSensitizations");
        }
    }
}
