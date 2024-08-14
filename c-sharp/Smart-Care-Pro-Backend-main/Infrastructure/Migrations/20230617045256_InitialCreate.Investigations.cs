using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateInvestigations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Investigations",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SampleCollectionDate = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SampleQuantity = table.Column<int>(type: "int", nullable: false),
                    Piority = table.Column<byte>(type: "tinyint", nullable: false),
                    ImagingTestDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResultReceived = table.Column<bool>(type: "bit", nullable: false),
                    ClinicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Investigations", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Investigations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Investigations_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Investigations_UserAccounts_ClinicianId",
                        column: x => x.ClinicianId,
                        principalTable: "UserAccounts",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_ClientId",
                table: "Investigations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_ClinicianId",
                table: "Investigations",
                column: "ClinicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_TestId",
                table: "Investigations",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investigations");
        }
    }
}
