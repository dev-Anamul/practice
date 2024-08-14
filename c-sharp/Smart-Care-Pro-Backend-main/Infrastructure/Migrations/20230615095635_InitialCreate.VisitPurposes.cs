using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateVisitPurposes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitPurposes",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitPurposes = table.Column<byte>(type: "tinyint", nullable: true),
                    ReasonForVisit = table.Column<byte>(type: "tinyint", nullable: true),
                    PregnancyIntension = table.Column<byte>(type: "tinyint", nullable: true),
                    ReasonForFollowUp = table.Column<byte>(type: "tinyint", nullable: true),
                    OtherReasonForFollowUp = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    ReasonForStopping = table.Column<byte>(type: "tinyint", nullable: true),
                    OtherReasonForStopping = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
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
                    table.PrimaryKey("PK_VisitPurposes", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_VisitPurposes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitPurposes_ClientId",
                table: "VisitPurposes",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitPurposes");
        }
    }
}
