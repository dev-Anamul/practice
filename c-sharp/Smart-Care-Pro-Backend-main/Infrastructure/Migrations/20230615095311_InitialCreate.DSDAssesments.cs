using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateDSDAssesments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DSDAssesments",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsClientStableOnCare = table.Column<bool>(type: "bit", nullable: false),
                    ShouldContinueDSD = table.Column<bool>(type: "bit", nullable: false),
                    ShouldReferToClinician = table.Column<bool>(type: "bit", nullable: false),
                    HealthPost = table.Column<bool>(type: "bit", nullable: false),
                    Scholar = table.Column<bool>(type: "bit", nullable: false),
                    MobileARTDistributionModel = table.Column<bool>(type: "bit", nullable: false),
                    RuralAdherenceModel = table.Column<bool>(type: "bit", nullable: false),
                    CommunityPost = table.Column<bool>(type: "bit", nullable: false),
                    FastTrack = table.Column<bool>(type: "bit", nullable: false),
                    Weekend = table.Column<bool>(type: "bit", nullable: false),
                    CentralDispensingUnit = table.Column<bool>(type: "bit", nullable: false),
                    CommunityARTDistributionPoints = table.Column<bool>(type: "bit", nullable: false),
                    Other = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
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
                    table.PrimaryKey("PK_DSDAssesments", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_DSDAssesments_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DSDAssesments_ClientId",
                table: "DSDAssesments",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DSDAssesments");
        }
    }
}
