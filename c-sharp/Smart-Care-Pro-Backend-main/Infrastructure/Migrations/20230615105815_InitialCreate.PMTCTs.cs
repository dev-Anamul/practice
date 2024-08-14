using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreatePMTCTs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PMTCTs",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasMotherTakenARV = table.Column<byte>(type: "tinyint", nullable: false),
                    OtherARVForMother = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WhenMotherTakenARV = table.Column<byte>(type: "tinyint", nullable: true),
                    ARVStartDateForMother = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ARVEndDateForMother = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HowLongMotherTakenARV = table.Column<int>(type: "int", nullable: true),
                    DurationUnitForMother = table.Column<byte>(type: "tinyint", nullable: true),
                    WhatARVMotherWasTaken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HasChildTakenARV = table.Column<byte>(type: "tinyint", nullable: false),
                    OtherARVForChild = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HowLongChildTakenARV = table.Column<byte>(type: "tinyint", nullable: true),
                    ARVStartDateForChild = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ARVEndDateForChild = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WhatARVChildWasTaken = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
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
                    table.PrimaryKey("PK_PMTCTs", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_PMTCTs_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PMTCTs_ClientId",
                table: "PMTCTs",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PMTCTs");
        }
    }
}
