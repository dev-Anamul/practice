using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateOptedVMMCCampaign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OptedVMMCCampaigns",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VMMCCampaignId = table.Column<int>(type: "int", nullable: false),
                    VMMCServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_OptedVMMCCampaigns", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_OptedVMMCCampaigns_VMMCCampaigns_VMMCCampaignId",
                        column: x => x.VMMCCampaignId,
                        principalTable: "VMMCCampaigns",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptedVMMCCampaigns_VMMCServices_VMMCServiceId",
                        column: x => x.VMMCServiceId,
                        principalTable: "VMMCServices",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OptedVMMCCampaigns_VMMCCampaignId",
                table: "OptedVMMCCampaigns",
                column: "VMMCCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_OptedVMMCCampaigns_VMMCServiceId",
                table: "OptedVMMCCampaigns",
                column: "VMMCServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OptedVMMCCampaigns");
        }
    }
}
