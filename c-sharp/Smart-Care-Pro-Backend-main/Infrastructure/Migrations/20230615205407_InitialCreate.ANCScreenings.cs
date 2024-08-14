using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateANCScreenings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ANCScreenings",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HistoryofBleeding = table.Column<bool>(type: "bit", nullable: false),
                    Draining = table.Column<bool>(type: "bit", nullable: false),
                    PVMucus = table.Column<bool>(type: "bit", nullable: false),
                    Contraction = table.Column<bool>(type: "bit", nullable: false),
                    PVBleeding = table.Column<bool>(type: "bit", nullable: false),
                    FetalMovements = table.Column<bool>(type: "bit", nullable: false),
                    IsSyphilisDone = table.Column<bool>(type: "bit", nullable: false),
                    SyphilisTestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SyphilisTestType = table.Column<byte>(type: "tinyint", nullable: false),
                    SyphilisResult = table.Column<byte>(type: "tinyint", nullable: false),
                    IsHepatitisDone = table.Column<bool>(type: "bit", nullable: false),
                    HepatitisTestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HepatitisTestType = table.Column<byte>(type: "tinyint", nullable: false),
                    HepatitisResult = table.Column<byte>(type: "tinyint", nullable: false),
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
                    table.PrimaryKey("PK_ANCScreenings", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ANCScreenings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ANCScreenings_ClientId",
                table: "ANCScreenings",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ANCScreenings");
        }
    }
}
