using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateIdentifiedReferralReasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentifiedReferralReasons",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferralModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReasonOfReferralId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_IdentifiedReferralReasons", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_IdentifiedReferralReasons_ReasonOfReferrals_ReasonOfReferralId",
                        column: x => x.ReasonOfReferralId,
                        principalTable: "ReasonOfReferrals",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentifiedReferralReasons_ReferralModules_ReferralModuleId",
                        column: x => x.ReferralModuleId,
                        principalTable: "ReferralModules",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedReferralReasons_ReasonOfReferralId",
                table: "IdentifiedReferralReasons",
                column: "ReasonOfReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedReferralReasons_ReferralModuleId",
                table: "IdentifiedReferralReasons",
                column: "ReferralModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentifiedReferralReasons");
        }
    }
}
