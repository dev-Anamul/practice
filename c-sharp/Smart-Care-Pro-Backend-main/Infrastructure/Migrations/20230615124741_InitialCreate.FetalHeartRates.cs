using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateFetalHeartRates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FetalHeartRates",
                columns: table => new
                {
                    FetalHeartRateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FetalRate = table.Column<int>(type: "int", nullable: false),
                    FetalRateTime = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_FetalHeartRates", x => x.FetalHeartRateID);
                    table.ForeignKey(
                        name: "FK_FetalHeartRates_Partograph_PartographID",
                        column: x => x.PartographID,
                        principalTable: "Partograph",
                        principalColumn: "PartographID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FetalHeartRates_PartographID",
                table: "FetalHeartRates",
                column: "PartographID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FetalHeartRates");
        }
    }
}
