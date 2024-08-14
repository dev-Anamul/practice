using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateFacility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacilityMasterCode",
                table: "Facilities",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "FacilityType",
                table: "Facilities",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Location",
                table: "Facilities",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Ownership",
                table: "Facilities",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacilityMasterCode",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "FacilityType",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Ownership",
                table: "Facilities");
        }
    }
}
