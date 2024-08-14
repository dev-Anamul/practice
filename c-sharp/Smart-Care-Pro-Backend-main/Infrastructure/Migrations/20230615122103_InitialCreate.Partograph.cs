using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreatePartograph : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partograph",
                columns: table => new
                {
                    PartographID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gravida = table.Column<byte>(type: "tinyint", nullable: false),
                    Parity = table.Column<byte>(type: "tinyint", nullable: false),
                    SBOrNND = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Abortion = table.Column<int>(type: "int", nullable: true),
                    EDD = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BorderlineRiskFactors = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegularContractions = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InitiateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InitiateTime = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MembranesRuptured = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdmissionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Partograph", x => x.PartographID);
                    table.ForeignKey(
                        name: "FK_Partograph_Encounters_EncounterID",
                        column: x => x.EncounterID,
                        principalTable: "Encounters",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partograph_EncounterID",
                table: "Partograph",
                column: "EncounterID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partograph");
        }
    }
}
