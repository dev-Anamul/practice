using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateServiceQueueModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServicePoints_Facilities_FacilityId",
                table: "ServicePoints");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceQueues_Facilities_FacilityId",
                table: "ServiceQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceQueues_ServicePoints_ServicePointId",
                table: "ServiceQueues");

            migrationBuilder.DropIndex(
                name: "IX_ServiceQueues_FacilityId",
                table: "ServiceQueues");

            migrationBuilder.DropIndex(
                name: "IX_ServicePoints_FacilityId",
                table: "ServicePoints");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "ServiceQueues");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "ServicePoints");

            migrationBuilder.DropColumn(
                name: "ServiceCode",
                table: "ServicePoints");

            migrationBuilder.RenameColumn(
                name: "ServicePointId",
                table: "ServiceQueues",
                newName: "FacilityQueueId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceQueues_ServicePointId",
                table: "ServiceQueues",
                newName: "IX_ServiceQueues_FacilityQueueId");

            migrationBuilder.AddColumn<byte>(
                name: "ServiceCode",
                table: "ServiceQueues",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "FacilityQueue",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceQueues_FacilityQueue_FacilityQueueId",
                table: "ServiceQueues");

            migrationBuilder.DropTable(
                name: "FacilityQueue");

            migrationBuilder.DropColumn(
                name: "ServiceCode",
                table: "ServiceQueues");

            migrationBuilder.RenameColumn(
                name: "FacilityQueueId",
                table: "ServiceQueues",
                newName: "ServicePointId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceQueues_FacilityQueueId",
                table: "ServiceQueues",
                newName: "IX_ServiceQueues_ServicePointId");

            migrationBuilder.AddColumn<int>(
                name: "FacilityId",
                table: "ServiceQueues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacilityId",
                table: "ServicePoints",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ServiceCode",
                table: "ServicePoints",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceQueues_FacilityId",
                table: "ServiceQueues",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePoints_FacilityId",
                table: "ServicePoints",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePoints_Facilities_FacilityId",
                table: "ServicePoints",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Oid");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceQueues_Facilities_FacilityId",
                table: "ServiceQueues",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceQueues_ServicePoints_ServicePointId",
                table: "ServiceQueues",
                column: "ServicePointId",
                principalTable: "ServicePoints",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
