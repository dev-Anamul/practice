using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateDFZPatientTypeAddPatientType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DFZPatientTypeId",
                table: "DFZClients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DFZClients_DFZPatientTypeId",
                table: "DFZClients",
                column: "DFZPatientTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DFZClients_DFZPatientTypes_DFZPatientTypeId",
                table: "DFZClients",
                column: "DFZPatientTypeId",
                principalTable: "DFZPatientTypes",
                principalColumn: "Oid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DFZClients_DFZPatientTypes_DFZPatientTypeId",
                table: "DFZClients");

            migrationBuilder.DropIndex(
                name: "IX_DFZClients_DFZPatientTypeId",
                table: "DFZClients");

            migrationBuilder.DropColumn(
                name: "DFZPatientTypeId",
                table: "DFZClients");
        }
    }
}
