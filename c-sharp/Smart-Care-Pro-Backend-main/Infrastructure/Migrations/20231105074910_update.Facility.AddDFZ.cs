using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class updateFacilityAddDFZ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Facilities",
                newName: "IsLive");

            migrationBuilder.AddColumn<bool>(
                name: "IsDFZ",
                table: "Facilities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDFZ",
                table: "Facilities");

            migrationBuilder.RenameColumn(
                name: "IsLive",
                table: "Facilities",
                newName: "IsActive");
        }
    }
}
