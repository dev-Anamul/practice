using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreatePatientStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientStatuses",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    OtherReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferralReason = table.Column<byte>(type: "tinyint", nullable: true),
                    ClinicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferringFacilityId = table.Column<int>(type: "int", nullable: true),
                    ReferredFacilityId = table.Column<int>(type: "int", nullable: true),
                    ReasonForInactive = table.Column<byte>(type: "tinyint", nullable: true),
                    ARVStoppingReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonForStoppingART = table.Column<byte>(type: "tinyint", nullable: true),
                    ReasonForReactivation = table.Column<byte>(type: "tinyint", nullable: true),
                    ARTRegisterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_PatientStatuses", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_PatientStatuses_ARTServices_ARTRegisterId",
                        column: x => x.ARTRegisterId,
                        principalTable: "ARTServices",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientStatuses_ARTRegisterId",
                table: "PatientStatuses",
                column: "ARTRegisterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientStatuses");
        }
    }
}
