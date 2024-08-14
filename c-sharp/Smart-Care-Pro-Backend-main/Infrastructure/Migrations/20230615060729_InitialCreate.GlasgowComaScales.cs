using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateGlasgowComaScales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlasgowComaScales",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EyeScore = table.Column<byte>(type: "tinyint", nullable: false),
                    VerbalScore = table.Column<byte>(type: "tinyint", nullable: false),
                    MotorScale = table.Column<byte>(type: "tinyint", nullable: false),
                    GlasgowComaScore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RightPupilsLightReactionSize = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RightPupilsLightReactionReaction = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    LeftPupilsLightReactionSize = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    LeftPupilsLightReactionReaction = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
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
                    table.PrimaryKey("PK_GlasgowComaScales", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_GlasgowComaScales_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlasgowComaScales_ClientId",
                table: "GlasgowComaScales",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlasgowComaScales");
        }
    }
}
