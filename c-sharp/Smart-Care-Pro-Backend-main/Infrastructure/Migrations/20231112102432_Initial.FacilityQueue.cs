using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialFacilityQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceQueues_FacilityQueue_FacilityQueueId",
                table: "ServiceQueues");

            migrationBuilder.DropTable(
                name: "FacilityQueue");

            migrationBuilder.CreateTable(
                name: "FacilityQueues",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FacilityQueues", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_FacilityQueues_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityQueues_FacilityId",
                table: "FacilityQueues",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceQueues_FacilityQueues_FacilityQueueId",
                table: "ServiceQueues",
                column: "FacilityQueueId",
                principalTable: "FacilityQueues",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceQueues_FacilityQueues_FacilityQueueId",
                table: "ServiceQueues");

            migrationBuilder.DropTable(
                name: "FacilityQueues");

            migrationBuilder.CreateTable(
                name: "FacilityQueue",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedIn = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedIn = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityQueue", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_FacilityQueue_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityQueue_FacilityId",
                table: "FacilityQueue",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceQueues_FacilityQueue_FacilityQueueId",
                table: "ServiceQueues",
                column: "FacilityQueueId",
                principalTable: "FacilityQueue",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
