using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateAttachedFacilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachedFacilities",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeOfEntry = table.Column<byte>(type: "tinyint", nullable: false),
                    DateAttached = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    AttachedFacilityId = table.Column<int>(type: "int", nullable: true),
                    SourceFacilityId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_AttachedFacilities", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_AttachedFacilities_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachedFacilities_Facilities_AttachedFacilityId",
                        column: x => x.AttachedFacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Oid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachedFacilities_AttachedFacilityId",
                table: "AttachedFacilities",
                column: "AttachedFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachedFacilities_ClientId",
                table: "AttachedFacilities",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachedFacilities");
        }
    }
}
