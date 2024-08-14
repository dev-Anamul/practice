using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateSurgeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Surgeries",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    BookingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    OperationTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OperationType = table.Column<byte>(type: "tinyint", nullable: false),
                    Surgeons = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BookingNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TimePatientWheeledTheater = table.Column<TimeSpan>(type: "time", nullable: false),
                    NursingPreOpPlan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurgicalCheckList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Team = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperationStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OperationEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SurgeryIndication = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OperationName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PostOpProcedure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureType = table.Column<byte>(type: "tinyint", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SutureType = table.Column<byte>(type: "tinyint", nullable: false),
                    Other = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsVMMCSurgery = table.Column<bool>(type: "bit", nullable: false),
                    WardId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Surgeries", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_Surgeries_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Surgeries_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Surgeries_ClientId",
                table: "Surgeries",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Surgeries_WardId",
                table: "Surgeries",
                column: "WardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Surgeries");
        }
    }
}
