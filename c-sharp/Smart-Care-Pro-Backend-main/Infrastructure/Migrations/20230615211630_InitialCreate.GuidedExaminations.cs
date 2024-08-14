using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateGuidedExaminations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuidedExaminations",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sores = table.Column<byte>(type: "tinyint", nullable: false),
                    AbnormalDischarge = table.Column<byte>(type: "tinyint", nullable: false),
                    Warts = table.Column<byte>(type: "tinyint", nullable: false),
                    OtherAbnormalities = table.Column<byte>(type: "tinyint", nullable: false),
                    Normal = table.Column<byte>(type: "tinyint", nullable: false),
                    VaginalMuscleTone = table.Column<byte>(type: "tinyint", nullable: false),
                    CervixColour = table.Column<byte>(type: "tinyint", nullable: false),
                    CervicalConsistency = table.Column<byte>(type: "tinyint", nullable: false),
                    FibroidPalpable = table.Column<byte>(type: "tinyint", nullable: false),
                    Scars = table.Column<byte>(type: "tinyint", nullable: false),
                    Masses = table.Column<byte>(type: "tinyint", nullable: false),
                    LiverPalpable = table.Column<byte>(type: "tinyint", nullable: false),
                    Tenderness = table.Column<byte>(type: "tinyint", nullable: false),
                    UterusPosition = table.Column<byte>(type: "tinyint", nullable: false),
                    Size = table.Column<byte>(type: "tinyint", nullable: false),
                    TenderAdnexa = table.Column<byte>(type: "tinyint", nullable: false),
                    OtherFindings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CervicalDischarge = table.Column<byte>(type: "tinyint", nullable: false),
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
                    table.PrimaryKey("PK_GuidedExaminations", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_GuidedExaminations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuidedExaminations_ClientId",
                table: "GuidedExaminations",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuidedExaminations");
        }
    }
}
