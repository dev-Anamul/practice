using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateTheroAblation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThermoAblationTreatmentMethods",
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
                    table.PrimaryKey("PK_ThermoAblationTreatmentMethods", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "ThermoAblations",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsConsentObtained = table.Column<bool>(type: "bit", nullable: false),
                    IsClientCounseled = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedTimeForProcedure = table.Column<short>(type: "Smallint", nullable: false),
                    ThermoAblationComment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ThermoAblationTreatmentMethodId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_ThermoAblations", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ThermoAblations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThermoAblations_ThermoAblationTreatmentMethods_ThermoAblationTreatmentMethodId",
                        column: x => x.ThermoAblationTreatmentMethodId,
                        principalTable: "ThermoAblationTreatmentMethods",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThermoAblations_ClientId",
                table: "ThermoAblations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ThermoAblations_ThermoAblationTreatmentMethodId",
                table: "ThermoAblations",
                column: "ThermoAblationTreatmentMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThermoAblations");

            migrationBuilder.DropTable(
                name: "ThermoAblationTreatmentMethods");
        }
    }
}
