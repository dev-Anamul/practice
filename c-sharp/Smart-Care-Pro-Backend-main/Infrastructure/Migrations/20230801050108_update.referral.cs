using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class updatereferral : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferralModules_ServicePoints_InternalReferralServicesId",
                table: "ReferralModules");

            migrationBuilder.DropIndex(
                name: "IX_ReferralModules_InternalReferralServicesId",
                table: "ReferralModules");

            migrationBuilder.DropColumn(
                name: "ExternalReferalServicesId",
                table: "ReferralModules");

            migrationBuilder.DropColumn(
                name: "InternalReferralServicesId",
                table: "ReferralModules");

            migrationBuilder.AddColumn<int>(
                name: "ServicePointId",
                table: "ReferralModules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReferralModules_ServicePointId",
                table: "ReferralModules",
                column: "ServicePointId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralModules_ServicePoints_ServicePointId",
                table: "ReferralModules",
                column: "ServicePointId",
                principalTable: "ServicePoints",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferralModules_ServicePoints_ServicePointId",
                table: "ReferralModules");

            migrationBuilder.DropIndex(
                name: "IX_ReferralModules_ServicePointId",
                table: "ReferralModules");

            migrationBuilder.DropColumn(
                name: "ServicePointId",
                table: "ReferralModules");

            migrationBuilder.AddColumn<int>(
                name: "ExternalReferalServicesId",
                table: "ReferralModules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InternalReferralServicesId",
                table: "ReferralModules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReferralModules_InternalReferralServicesId",
                table: "ReferralModules",
                column: "InternalReferralServicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralModules_ServicePoints_InternalReferralServicesId",
                table: "ReferralModules",
                column: "InternalReferralServicesId",
                principalTable: "ServicePoints",
                principalColumn: "Oid");
        }
    }
}
