using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class IntialCreateBirthRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BirthRecords",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsBirthRecordGiven = table.Column<bool>(type: "bit", nullable: false),
                    IsUnderFiveCardGiven = table.Column<bool>(type: "bit", nullable: false),
                    UnderFiveCardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Origin = table.Column<byte>(type: "tinyint", nullable: false),
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
                    table.PrimaryKey("PK_BirthRecords", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_BirthRecords_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BirthRecords_ClientId",
                table: "BirthRecords",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BirthRecords");
        }
    }
}
