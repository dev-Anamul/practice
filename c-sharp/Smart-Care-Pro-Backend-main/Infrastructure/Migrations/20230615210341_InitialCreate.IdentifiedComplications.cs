using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateIdentifiedComplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentifiedComplications",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplicationTypeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_IdentifiedComplications", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_IdentifiedComplications_Complications_ComplicationId",
                        column: x => x.ComplicationId,
                        principalTable: "Complications",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IdentifiedComplications_ComplicationTypes_ComplicationTypeId",
                        column: x => x.ComplicationTypeId,
                        principalTable: "ComplicationTypes",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedComplications_ComplicationId",
                table: "IdentifiedComplications",
                column: "ComplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentifiedComplications_ComplicationTypeId",
                table: "IdentifiedComplications",
                column: "ComplicationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentifiedComplications");
        }
    }
}
