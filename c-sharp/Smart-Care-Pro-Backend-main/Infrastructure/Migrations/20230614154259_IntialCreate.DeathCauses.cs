using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class IntialCreateDeathCauses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeathCauses",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CauseType = table.Column<byte>(type: "tinyint", nullable: false),
                    ICPC2Id = table.Column<int>(type: "int", nullable: false),
                    ICD11Id = table.Column<int>(type: "int", nullable: true),
                    DeathRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_DeathCauses", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_DeathCauses_DeathRecords_DeathRecordId",
                        column: x => x.DeathRecordId,
                        principalTable: "DeathRecords",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeathCauses_ICDDiagnoses_ICD11Id",
                        column: x => x.ICD11Id,
                        principalTable: "ICDDiagnoses",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_DeathCauses_ICPC2Descriptions_ICPC2Id",
                        column: x => x.ICPC2Id,
                        principalTable: "ICPC2Descriptions",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeathCauses_DeathRecordId",
                table: "DeathCauses",
                column: "DeathRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_DeathCauses_ICD11Id",
                table: "DeathCauses",
                column: "ICD11Id");

            migrationBuilder.CreateIndex(
                name: "IX_DeathCauses_ICPC2Id",
                table: "DeathCauses",
                column: "ICPC2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeathCauses");
        }
    }
}
