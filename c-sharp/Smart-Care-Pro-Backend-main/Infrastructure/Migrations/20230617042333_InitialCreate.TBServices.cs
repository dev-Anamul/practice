using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateTBServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBServices",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseIdNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsHealthCareWorker = table.Column<bool>(type: "bit", nullable: false),
                    IsInmate = table.Column<bool>(type: "bit", nullable: false),
                    IsMiner = table.Column<bool>(type: "bit", nullable: false),
                    IsExMiner = table.Column<bool>(type: "bit", nullable: false),
                    OtherPatientCategory = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    TreatmentStarted = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    DateDischarged = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    TreatmentOutcome = table.Column<byte>(type: "tinyint", nullable: true),
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
                    table.PrimaryKey("PK_TBServices", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_TBServices_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBServices_ClientId",
                table: "TBServices",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBServices");
        }
    }
}
