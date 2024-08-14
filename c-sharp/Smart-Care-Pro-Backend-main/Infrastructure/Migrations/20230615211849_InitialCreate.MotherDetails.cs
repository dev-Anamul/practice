using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateMotherDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MotherDetails",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PregnancyNo = table.Column<int>(type: "int", nullable: false),
                    DateofDelivary = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MetarnalOutcome = table.Column<byte>(type: "tinyint", nullable: false),
                    PregnancyConclusion = table.Column<byte>(type: "tinyint", nullable: false),
                    EarlyTerminationReason = table.Column<byte>(type: "tinyint", nullable: false),
                    PregnancyDuration = table.Column<int>(type: "int", nullable: false),
                    MetarnalComplication = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    DeliveryMethod = table.Column<byte>(type: "tinyint", nullable: false),
                    PueperiumOutcome = table.Column<byte>(type: "tinyint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_MotherDetails", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_MotherDetails_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotherDetails_ClientId",
                table: "MotherDetails",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotherDetails");
        }
    }
}
