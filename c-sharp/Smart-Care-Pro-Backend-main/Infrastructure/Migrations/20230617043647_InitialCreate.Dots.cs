using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateDots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dots",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DotStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DotEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DotPlan = table.Column<byte>(type: "tinyint", nullable: false),
                    DiseaseSite = table.Column<byte>(type: "tinyint", nullable: false),
                    TBType = table.Column<byte>(type: "tinyint", nullable: false),
                    SusceptiblePTType = table.Column<byte>(type: "tinyint", nullable: false),
                    TBSusceptibleRegimen = table.Column<byte>(type: "tinyint", nullable: false),
                    MDRDRRegimenGroup = table.Column<byte>(type: "tinyint", nullable: false),
                    MDRDRRegimen = table.Column<byte>(type: "tinyint", nullable: false),
                    Phase = table.Column<byte>(type: "tinyint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TBServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_Dots", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Dots_TBServices_TBServiceId",
                        column: x => x.TBServiceId,
                        principalTable: "TBServices",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dots_TBServiceId",
                table: "Dots",
                column: "TBServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dots");
        }
    }
}
