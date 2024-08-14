using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateBirthHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BirthHistories",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthWeight = table.Column<short>(type: "Smallint", nullable: false),
                    BirthHeight = table.Column<short>(type: "Smallint", nullable: true),
                    BirthOutcome = table.Column<byte>(type: "tinyint", nullable: false),
                    HeadCircumference = table.Column<short>(type: "Smallint", nullable: true),
                    ChestCircumference = table.Column<short>(type: "Smallint", nullable: true),
                    GeneralCondition = table.Column<byte>(type: "tinyint", nullable: false),
                    IsBreastFeedingWell = table.Column<bool>(type: "bit", nullable: false),
                    OtherFeedingOption = table.Column<byte>(type: "tinyint", nullable: true),
                    DeliveryTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    VaccinationOutside = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TetanusAtBirth = table.Column<byte>(type: "tinyint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_BirthHistories", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_BirthHistories_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BirthHistories_ClientId",
                table: "BirthHistories",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BirthHistories");
        }
    }
}
