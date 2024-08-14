using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class IntialCreateDeathRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeathRecords",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InformantFirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    InformantSurname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    InformantNickname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    InformantRelationship = table.Column<byte>(type: "tinyint", nullable: false),
                    InformantOtherRelationship = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    InformantCity = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    InformantStreetNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    InformantPOBox = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    InformantLandmark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InformantLandlineCountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    InformantLandline = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    InformantCellphoneCountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    InformantCellphone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfDeath = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    AgeOfDeceased = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    WhereDeathOccured = table.Column<byte>(type: "tinyint", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistrictOfDeath = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_DeathRecords", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_DeathRecords_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeathRecords_Districts_DistrictOfDeath",
                        column: x => x.DistrictOfDeath,
                        principalTable: "Districts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeathRecords_ClientId",
                table: "DeathRecords",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DeathRecords_DistrictOfDeath",
                table: "DeathRecords",
                column: "DistrictOfDeath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeathRecords");
        }
    }
}
