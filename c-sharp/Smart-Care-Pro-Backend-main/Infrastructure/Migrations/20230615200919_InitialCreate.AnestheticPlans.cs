using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateAnestheticPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnestheticPlans",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientHistory = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ClientExamination = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AnestheticPlans = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PatientInstructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AnesthesiaStartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    AnesthesiaEndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    PostAnesthesia = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PreOperativeAdverse = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PostOperative = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SurgeryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_AnestheticPlans", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_AnestheticPlans_Surgeries_SurgeryId",
                        column: x => x.SurgeryId,
                        principalTable: "Surgeries",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnestheticPlans_SurgeryId",
                table: "AnestheticPlans",
                column: "SurgeryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnestheticPlans");
        }
    }
}
