using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateIdentifiedPregnencyConfirmations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentifiedPregnencyConfirmations",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GynConfirmationId = table.Column<int>(type: "int", nullable: false),
                    PregnancyBookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_IdentifiedPregnencyConfirmations", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_IdentifiedPregnencyConfirmations_GynConfirmations_GynConfirmationId",
                        column: x => x.GynConfirmationId,
                        principalTable: "GynConfirmations",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentifiedPregnencyConfirmations_PregnancyBookings_PregnancyBookingId",
                        column: x => x.PregnancyBookingId,
                        principalTable: "PregnancyBookings",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedPregnencyConfirmations_GynConfirmationId",
                table: "IdentifiedPregnencyConfirmations",
                column: "GynConfirmationId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedPregnencyConfirmations_PregnancyBookingId",
                table: "IdentifiedPregnencyConfirmations",
                column: "PregnancyBookingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentifiedPregnencyConfirmations");
        }
    }
}
