using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateARTTreatmentPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ARTTreatmentPlans",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtPlan = table.Column<byte>(type: "tinyint", nullable: true),
                    TPTPlan = table.Column<byte>(type: "tinyint", nullable: true),
                    CTXPlan = table.Column<byte>(type: "tinyint", nullable: true),
                    EACPlan = table.Column<byte>(type: "tinyint", nullable: true),
                    DSDPlan = table.Column<byte>(type: "tinyint", nullable: true),
                    FluconazolePlan = table.Column<byte>(type: "tinyint", nullable: true),
                    TPTEligibleToday = table.Column<bool>(type: "bit", nullable: false),
                    HaveTPTProvidedToday = table.Column<byte>(type: "tinyint", nullable: true),
                    TPTNote = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
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
                    table.PrimaryKey("PK_ARTTreatmentPlans", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ARTTreatmentPlans_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ARTTreatmentPlans_ClientId",
                table: "ARTTreatmentPlans",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ARTTreatmentPlans");
        }
    }
}
