using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConditionType = table.Column<byte>(type: "tinyint", nullable: false),
                    DateDiagnosed = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    DateResolved = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    IsOngoing = table.Column<bool>(type: "bit", nullable: false),
                    Certainty = table.Column<byte>(type: "tinyint", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NTGId = table.Column<int>(type: "int", nullable: true),
                    ICDId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Conditions", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Conditions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conditions_ICDDiagnoses_ICDId",
                        column: x => x.ICDId,
                        principalTable: "ICDDiagnoses",
                        principalColumn: "Oid");
                    table.ForeignKey(
                        name: "FK_Conditions_NTGLevelThreeDiagnoses_NTGId",
                        column: x => x.NTGId,
                        principalTable: "NTGLevelThreeDiagnoses",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_ClientId",
                table: "Conditions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_ICDId",
                table: "Conditions",
                column: "ICDId");

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_NTGId",
                table: "Conditions",
                column: "NTGId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conditions");
        }
    }
}
