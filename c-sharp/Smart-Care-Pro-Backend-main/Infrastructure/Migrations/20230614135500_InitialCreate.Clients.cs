using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Oid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Sex = table.Column<byte>(type: "tinyint", nullable: false),
                    DOB = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    IsDOBEstimated = table.Column<bool>(type: "bit", nullable: false),
                    NRC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    NoNRC = table.Column<bool>(type: "bit", nullable: false),
                    NAPSANumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UnderFiveCardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NUPN = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    FathersFirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    FathersSurname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    FathersNRC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    FatherNAPSANumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FatherNationality = table.Column<int>(type: "int", nullable: true),
                    IsFatherDeceased = table.Column<bool>(type: "bit", nullable: false),
                    MothersFirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    MothersSurname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    MothersNRC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    MotherNAPSANumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MotherNationality = table.Column<int>(type: "int", nullable: true),
                    IsMotherDeceased = table.Column<bool>(type: "bit", nullable: false),
                    GuardiansFirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    GuardiansSurname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    GuardiansNRC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    GuardianNAPSANumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GuardianNationality = table.Column<int>(type: "int", nullable: true),
                    GuardianRelationship = table.Column<byte>(type: "tinyint", nullable: true),
                    SpousesLegalName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    SpousesSurname = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    MaritalStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    CellphoneCountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Cellphone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    OtherCellphoneCountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    OtherCellphone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    NoCellphone = table.Column<bool>(type: "bit", nullable: false),
                    LandlineCountryCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Landline = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    HouseholdNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Road = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    Area = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    Landmarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsZambianBorn = table.Column<bool>(type: "bit", nullable: false),
                    BirthPlace = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    TownName = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    Religion = table.Column<byte>(type: "tinyint", nullable: true),
                    HIVStatus = table.Column<byte>(type: "tinyint", nullable: true),
                    IsDeceased = table.Column<bool>(type: "bit", nullable: false),
                    IsOnART = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmitted = table.Column<bool>(type: "bit", nullable: false),
                    MotherProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FatherProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: true),
                    HomeLanguageId = table.Column<int>(type: "int", nullable: false),
                    EducationLevelId = table.Column<int>(type: "int", nullable: true),
                    OccupationId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Clients", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_Clients_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Clients_EducationLevels_EducationLevelId",
                        column: x => x.EducationLevelId,
                        principalTable: "EducationLevels",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Clients_HomeLanguages_HomeLanguageId",
                        column: x => x.HomeLanguageId,
                        principalTable: "HomeLanguages",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_Occupations_OccupationId",
                        column: x => x.OccupationId,
                        principalTable: "Occupations",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CountryId",
                table: "Clients",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_DistrictId",
                table: "Clients",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_EducationLevelId",
                table: "Clients",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_HomeLanguageId",
                table: "Clients",
                column: "HomeLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_OccupationId",
                table: "Clients",
                column: "OccupationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
