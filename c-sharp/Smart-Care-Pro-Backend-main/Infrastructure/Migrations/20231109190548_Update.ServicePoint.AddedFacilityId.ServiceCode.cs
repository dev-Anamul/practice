using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateServicePointAddedFacilityIdServiceCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_ServicePoints_FacilityId",
                table: "ServicePoints",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePoints_Facilities_FacilityId",
                table: "ServicePoints",
                column: "FacilityId",
                principalTable: "Facilities",
                principalColumn: "Oid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServicePoints_Facilities_FacilityId",
                table: "ServicePoints");

            migrationBuilder.DropIndex(
                name: "IX_ServicePoints_FacilityId",
                table: "ServicePoints");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "ServicePoints");

            migrationBuilder.DropColumn(
                name: "ServiceCode",
                table: "ServicePoints");
        }
    }
}
