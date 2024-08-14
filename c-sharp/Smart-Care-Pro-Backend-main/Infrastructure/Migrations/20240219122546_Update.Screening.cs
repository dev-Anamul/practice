using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateScreening : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "MenstrualCycleRegularity",
                table: "GynObsHistories",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "Tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "MenstrualBloodFlow",
                table: "GynObsHistories",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "Tinyint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "MenstrualCycleRegularity",
                table: "GynObsHistories",
                type: "Tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "MenstrualBloodFlow",
                table: "GynObsHistories",
                type: "Tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);
        }
    }
}
