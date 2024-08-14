using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateVolumes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Volumes",
                columns: table => new
                {
                    VolumesID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VolumesDetails = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VolumesTime = table.Column<long>(type: "bigint", nullable: false),
                    PartographID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedIn = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedIn = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volumes", x => x.VolumesID);
                    table.ForeignKey(
                        name: "FK_Volumes_Partograph_PartographID",
                        column: x => x.PartographID,
                        principalTable: "Partograph",
                        principalColumn: "PartographID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Volumes_PartographID",
                table: "Volumes",
                column: "PartographID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Volumes");
        }
    }
}
