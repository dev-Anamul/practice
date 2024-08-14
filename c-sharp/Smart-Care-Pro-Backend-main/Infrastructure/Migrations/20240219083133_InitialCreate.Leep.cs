using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateLeep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeepsTreatmentMethods",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
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
                    table.PrimaryKey("PK_LeepsTreatmentMethods", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "Leeps",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsLesionExtendsIntoTheCervicalOs = table.Column<bool>(type: "bit", nullable: false),
                    IsQueryICC = table.Column<bool>(type: "bit", nullable: false),
                    IsAtypicalVessels = table.Column<bool>(type: "bit", nullable: false),
                    IsPunctationsOrMoasicm = table.Column<bool>(type: "bit", nullable: false),
                    IsLesionCovers = table.Column<bool>(type: "bit", nullable: false),
                    IsPoly = table.Column<bool>(type: "bit", nullable: false),
                    IsLesionTooLargeForThermoAblation = table.Column<bool>(type: "bit", nullable: false),
                    IsLesionToThickForAblation = table.Column<bool>(type: "bit", nullable: false),
                    IsPostLEEPLesion = table.Column<bool>(type: "bit", nullable: false),
                    IsConsentObtained = table.Column<bool>(type: "bit", nullable: false),
                    IsClientCounseled = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedTimeForProcedure = table.Column<short>(type: "Smallint", nullable: true),
                    AssesmentComment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProcedureComment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LeepTreatmentMethodId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Leeps", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Leeps_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leeps_LeepsTreatmentMethods_LeepTreatmentMethodId",
                        column: x => x.LeepTreatmentMethodId,
                        principalTable: "LeepsTreatmentMethods",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leeps_ClientId",
                table: "Leeps",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Leeps_LeepTreatmentMethodId",
                table: "Leeps",
                column: "LeepTreatmentMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leeps");

            migrationBuilder.DropTable(
                name: "LeepsTreatmentMethods");
        }
    }
}
