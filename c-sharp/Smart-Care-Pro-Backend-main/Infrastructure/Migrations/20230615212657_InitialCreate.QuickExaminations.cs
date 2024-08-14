using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateQuickExaminations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuickExaminations",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HairWellSpread = table.Column<byte>(type: "tinyint", nullable: false),
                    HairHealthy = table.Column<byte>(type: "tinyint", nullable: false),
                    HeadInfection = table.Column<byte>(type: "tinyint", nullable: false),
                    Thrash = table.Column<byte>(type: "tinyint", nullable: false),
                    DentalDisease = table.Column<byte>(type: "tinyint", nullable: false),
                    CervicalGlandsEnlarged = table.Column<byte>(type: "tinyint", nullable: false),
                    NeckAbnormal = table.Column<byte>(type: "tinyint", nullable: false),
                    BreastLumps = table.Column<byte>(type: "tinyint", nullable: false),
                    Armpits = table.Column<byte>(type: "tinyint", nullable: false),
                    FibroidPalpable = table.Column<byte>(type: "tinyint", nullable: false),
                    Scars = table.Column<byte>(type: "tinyint", nullable: false),
                    LiverPalpable = table.Column<byte>(type: "tinyint", nullable: false),
                    Tenderness = table.Column<byte>(type: "tinyint", nullable: false),
                    Masses = table.Column<byte>(type: "tinyint", nullable: false),
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
                    table.PrimaryKey("PK_QuickExaminations", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_QuickExaminations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuickExaminations_ClientId",
                table: "QuickExaminations",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuickExaminations");
        }
    }
}
