using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateNeonatalDeaths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NeonatalDeaths",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeOfDeath = table.Column<TimeSpan>(type: "time", nullable: true),
                    AgeAtTimeOfDeath = table.Column<int>(type: "int", nullable: true),
                    TimeUnit = table.Column<byte>(type: "tinyint", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Other = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    CauseOfNeonatalDeathId = table.Column<int>(type: "int", nullable: true),
                    NeonatalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_NeonatalDeaths", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_NeonatalDeaths_CauseOfNeonatalDeaths_CauseOfNeonatalDeathId",
                        column: x => x.CauseOfNeonatalDeathId,
                        principalTable: "CauseOfNeonatalDeaths",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_NeonatalDeaths_NewBornDetails_NeonatalId",
                        column: x => x.NeonatalId,
                        principalTable: "NewBornDetails",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NeonatalDeaths_CauseOfNeonatalDeathId",
                table: "NeonatalDeaths",
                column: "CauseOfNeonatalDeathId");

            migrationBuilder.CreateIndex(
                name: "IX_NeonatalDeaths_NeonatalId",
                table: "NeonatalDeaths",
                column: "NeonatalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NeonatalDeaths");
        }
    }
}
