using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initialmigrationformodifydatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "IsDependent",
                table: "DFZPatientTypes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "DFZPatientTypeId",
                table: "DFZDependents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HospitalNo",
                table: "DFZDependents",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DFZDependents_DFZPatientTypeId",
                table: "DFZDependents",
                column: "DFZPatientTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DFZDependents_DFZPatientTypes_DFZPatientTypeId",
                table: "DFZDependents",
                column: "DFZPatientTypeId",
                principalTable: "DFZPatientTypes",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DFZDependents_DFZPatientTypes_DFZPatientTypeId",
                table: "DFZDependents");

            migrationBuilder.DropIndex(
                name: "IX_DFZDependents_DFZPatientTypeId",
                table: "DFZDependents");

            migrationBuilder.DropColumn(
                name: "IsDependent",
                table: "DFZPatientTypes");

            migrationBuilder.DropColumn(
                name: "DFZPatientTypeId",
                table: "DFZDependents");

            migrationBuilder.DropColumn(
                name: "HospitalNo",
                table: "DFZDependents");
        }
    }
}
