using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateChiefComplaints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiefComplaints",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChiefComplaints = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HistoryOfChiefComplaint = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HistorySummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExaminationSummary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HIVStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    LastHIVTestDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    TestingLocation = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    PotentialHIVExposureDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    RecencyType = table.Column<byte>(type: "tinyint", nullable: true),
                    RecencyTestDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ChildExposureStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    IsChildGivenARV = table.Column<bool>(type: "bit", nullable: false),
                    IsMotherGivenARV = table.Column<bool>(type: "bit", nullable: false),
                    NATTestDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    NATResult = table.Column<byte>(type: "tinyint", nullable: false),
                    TBScreenings = table.Column<byte>(type: "tinyint", nullable: true),
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
                    table.PrimaryKey("PK_ChiefComplaints", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ChiefComplaints_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiefComplaints_ClientId",
                table: "ChiefComplaints",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiefComplaints");
        }
    }
}
