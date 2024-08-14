using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateContraceptiveHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContraceptiveHistories",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContraceptiveId = table.Column<int>(type: "int", nullable: false),
                    GynObsHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ContraceptiveHistories", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ContraceptiveHistories_Contraceptives_ContraceptiveId",
                        column: x => x.ContraceptiveId,
                        principalTable: "Contraceptives",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContraceptiveHistories_GynObsHistories_GynObsHistoryId",
                        column: x => x.GynObsHistoryId,
                        principalTable: "GynObsHistories",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContraceptiveHistories_ContraceptiveId",
                table: "ContraceptiveHistories",
                column: "ContraceptiveId");

            migrationBuilder.CreateIndex(
                name: "IX_ContraceptiveHistories_GynObsHistoryId",
                table: "ContraceptiveHistories",
                column: "GynObsHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContraceptiveHistories");
        }
    }
}
