using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateUsedTBIdentificationMethods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsedTBIdentificationMethods",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TBIdentificationMethodId = table.Column<int>(type: "int", nullable: false),
                    TBHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_UsedTBIdentificationMethods", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_UsedTBIdentificationMethods_TBHistories_TBHistoryId",
                        column: x => x.TBHistoryId,
                        principalTable: "TBHistories",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsedTBIdentificationMethods_TBIdentificationMethods_TBIdentificationMethodId",
                        column: x => x.TBIdentificationMethodId,
                        principalTable: "TBIdentificationMethods",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsedTBIdentificationMethods_TBHistoryId",
                table: "UsedTBIdentificationMethods",
                column: "TBHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedTBIdentificationMethods_TBIdentificationMethodId",
                table: "UsedTBIdentificationMethods",
                column: "TBIdentificationMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsedTBIdentificationMethods");
        }
    }
}
