using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateMotherDeliverySummaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MotherDeliverySummaries",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    BirthType = table.Column<byte>(type: "tinyint", nullable: false),
                    GestationalPeriod = table.Column<int>(type: "int", nullable: false),
                    DeliveredType = table.Column<byte>(type: "tinyint", nullable: false),
                    LaborDuration = table.Column<int>(type: "int", nullable: false),
                    DeliveryLocation = table.Column<byte>(type: "tinyint", nullable: false),
                    DurationOfRupture = table.Column<int>(type: "int", nullable: false),
                    DeliveredByName = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    DeliveredBy = table.Column<byte>(type: "tinyint", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    VaginalWashes = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_MotherDeliverySummaries", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_MotherDeliverySummaries_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotherDeliverySummaries_ClientId",
                table: "MotherDeliverySummaries",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotherDeliverySummaries");
        }
    }
}
