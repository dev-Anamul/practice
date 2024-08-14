using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateFamilyPlanRegisters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FamilyPlanRegisters",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Referredby = table.Column<byte>(type: "tinyint", nullable: false),
                    OtherReferrals = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    FamilyPlanningYear = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    ClientStaysWith = table.Column<byte>(type: "tinyint", nullable: false),
                    CommunicationConsent = table.Column<byte>(type: "tinyint", nullable: false),
                    CommunicationPreference = table.Column<byte>(type: "tinyint", nullable: false),
                    TypeOfAlternativeContacts = table.Column<byte>(type: "tinyint", nullable: false),
                    OtherAlternativeContacts = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
                    ContactPhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    ContactAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PatientType = table.Column<byte>(type: "tinyint", nullable: false),
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
                    table.PrimaryKey("PK_FamilyPlanRegisters", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_FamilyPlanRegisters_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyPlanRegisters_ClientId",
                table: "FamilyPlanRegisters",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamilyPlanRegisters");
        }
    }
}
