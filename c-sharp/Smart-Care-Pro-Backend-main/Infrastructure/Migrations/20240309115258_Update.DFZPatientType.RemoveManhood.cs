using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateDFZPatientTypeRemoveManhood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManNumber",
                table: "DFZClients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManNumber",
                table: "DFZClients",
                type: "nvarchar(90)",
                maxLength: 90,
                nullable: true);
        }
    }
}
