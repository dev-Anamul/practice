using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateChildDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChildDetails",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthOutcome = table.Column<byte>(type: "tinyint", nullable: false),
                    BirthWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChildName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ChildSex = table.Column<byte>(type: "tinyint", nullable: false),
                    LastTCDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastTCResult = table.Column<byte>(type: "tinyint", nullable: false),
                    ChildCareNumber = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    DateOfChildTurns18Months = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_ChildDetails", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_ChildDetails_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildDetails_ClientId",
                table: "ChildDetails",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildDetails");
        }
    }
}
