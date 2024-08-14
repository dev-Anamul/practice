using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class IntialCreateEncounters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterType = table.Column<byte>(type: "tinyint", nullable: true),
                    OPDVisitDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    IPDAdmissionDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    AdmissionNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdmissionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferralDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPDDischargeDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    DischargeNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BedId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Encounters", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Encounters_Beds_BedId",
                        column: x => x.BedId,
                        principalTable: "Beds",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Encounters_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_BedId",
                table: "Encounters",
                column: "BedId");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_ClientId",
                table: "Encounters",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Encounters");
        }
    }
}
