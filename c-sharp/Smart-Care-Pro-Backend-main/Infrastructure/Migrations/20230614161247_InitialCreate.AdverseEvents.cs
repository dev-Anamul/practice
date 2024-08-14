using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateAdverseEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdverseEvents",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AEFIDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Swelling = table.Column<bool>(type: "bit", nullable: false),
                    Joint = table.Column<bool>(type: "bit", nullable: false),
                    Malaise = table.Column<bool>(type: "bit", nullable: false),
                    BodyAches = table.Column<bool>(type: "bit", nullable: false),
                    Fever = table.Column<bool>(type: "bit", nullable: false),
                    AllergicReaction = table.Column<bool>(type: "bit", nullable: false),
                    OtherAdverseEvent = table.Column<bool>(type: "bit", nullable: false),
                    OtherAEFI = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImmunizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_AdverseEvents", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_AdverseEvents_ImmunizationRecords_ImmunizationId",
                        column: x => x.ImmunizationId,
                        principalTable: "ImmunizationRecords",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdverseEvents_ImmunizationId",
                table: "AdverseEvents",
                column: "ImmunizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdverseEvents");
        }
    }
}
