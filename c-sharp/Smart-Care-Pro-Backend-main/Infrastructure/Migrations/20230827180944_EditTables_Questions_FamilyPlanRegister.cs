using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class EditTables_Questions_FamilyPlanRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TemperatureId",
                table: "Temperatures",
                newName: "InteractionId");

            migrationBuilder.RenameColumn(
                name: "ContractionsId",
                table: "Contractions",
                newName: "InteractionId");

            migrationBuilder.RenameColumn(
                name: "CervixId",
                table: "Cervixes",
                newName: "InteractionId");

            migrationBuilder.RenameColumn(
                name: "BloodPressureId",
                table: "BloodPressures",
                newName: "InteractionId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ContactName",
                table: "FamilyPlanRegisters",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Temperatures",
                newName: "TemperatureId");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Contractions",
                newName: "ContractionsId");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "Cervixes",
                newName: "CervixId");

            migrationBuilder.RenameColumn(
                name: "InteractionId",
                table: "BloodPressures",
                newName: "BloodPressureId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Questions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactName",
                table: "FamilyPlanRegisters",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90,
                oldNullable: true);
        }
    }
}
