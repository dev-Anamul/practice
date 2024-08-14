using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateServiceQueueAddUrgencyTypeServicePointId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUrgent",
                table: "ServiceQueues");

            migrationBuilder.RenameColumn(
                name: "ServiceCode",
                table: "ServiceQueues",
                newName: "UrgencyType");

            migrationBuilder.AddColumn<int>(
                name: "ServicePointId",
                table: "ServiceQueues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceQueues_ServicePointId",
                table: "ServiceQueues",
                column: "ServicePointId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceQueues_ServicePoints_ServicePointId",
                table: "ServiceQueues",
                column: "ServicePointId",
                principalTable: "ServicePoints",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceQueues_ServicePoints_ServicePointId",
                table: "ServiceQueues");

            migrationBuilder.DropIndex(
                name: "IX_ServiceQueues_ServicePointId",
                table: "ServiceQueues");

            migrationBuilder.DropColumn(
                name: "ServicePointId",
                table: "ServiceQueues");

            migrationBuilder.RenameColumn(
                name: "UrgencyType",
                table: "ServiceQueues",
                newName: "ServiceCode");

            migrationBuilder.AddColumn<bool>(
                name: "IsUrgent",
                table: "ServiceQueues",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
