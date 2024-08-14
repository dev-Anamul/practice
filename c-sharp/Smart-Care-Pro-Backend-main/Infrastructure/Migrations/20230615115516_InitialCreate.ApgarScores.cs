using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateApgarScores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApgarScores",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApgarTimeSpan = table.Column<byte>(type: "tinyint", nullable: false),
                    Appearance = table.Column<byte>(type: "tinyint", nullable: false),
                    Pulses = table.Column<byte>(type: "tinyint", nullable: false),
                    Grimace = table.Column<byte>(type: "tinyint", nullable: false),
                    Activity = table.Column<byte>(type: "tinyint", nullable: false),
                    Respiration = table.Column<byte>(type: "tinyint", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ApgarScores", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ApgarScores_NewBornDetails_NeonatalId",
                        column: x => x.NeonatalId,
                        principalTable: "NewBornDetails",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApgarScores_NeonatalId",
                table: "ApgarScores",
                column: "NeonatalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApgarScores");
        }
    }
}
