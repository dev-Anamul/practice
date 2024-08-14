using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateReferralModules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReferralModules",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsProceededFacility = table.Column<bool>(type: "bit", nullable: true),
                    ReasonForReferral = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferralType = table.Column<byte>(type: "tinyint", nullable: true),
                    OtherFacility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherFacilityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalReferralServicesId = table.Column<int>(type: "int", nullable: true),
                    ExternalReferalServicesId = table.Column<int>(type: "int", nullable: true),
                    ReferredFacilityId = table.Column<int>(type: "int", nullable: true),
                    ReceivingFacilityId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_ReferralModules", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ReferralModules_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferralModules_Facilities_ReceivingFacilityId",
                        column: x => x.ReceivingFacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_ReferralModules_ServicePoints_InternalReferralServicesId",
                        column: x => x.InternalReferralServicesId,
                        principalTable: "ServicePoints",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferralModules_ClientId",
                table: "ReferralModules",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralModules_InternalReferralServicesId",
                table: "ReferralModules",
                column: "InternalReferralServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralModules_ReceivingFacilityId",
                table: "ReferralModules",
                column: "ReceivingFacilityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferralModules");
        }
    }
}
