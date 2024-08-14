using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateDFZClientADD_ManNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DFZClients_DFZRanks_DFZRankId",
                table: "DFZClients");

            migrationBuilder.AddColumn<bool>(
                name: "IsDispencedPasserBy",
                table: "DispensedItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "DFZClients",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90);

            migrationBuilder.AlterColumn<string>(
                name: "ServiceNo",
                table: "DFZClients",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90);

            migrationBuilder.AlterColumn<int>(
                name: "DFZRankId",
                table: "DFZClients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ManNumber",
                table: "DFZClients",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DFZClients_DFZRanks_DFZRankId",
                table: "DFZClients",
                column: "DFZRankId",
                principalTable: "DFZRanks",
                principalColumn: "Oid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DFZClients_DFZRanks_DFZRankId",
                table: "DFZClients");

            migrationBuilder.DropColumn(
                name: "IsDispencedPasserBy",
                table: "DispensedItems");

            migrationBuilder.DropColumn(
                name: "ManNumber",
                table: "DFZClients");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "DFZClients",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ServiceNo",
                table: "DFZClients",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(90)",
                oldMaxLength: 90,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DFZRankId",
                table: "DFZClients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DFZClients_DFZRanks_DFZRankId",
                table: "DFZClients",
                column: "DFZRankId",
                principalTable: "DFZRanks",
                principalColumn: "Oid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
