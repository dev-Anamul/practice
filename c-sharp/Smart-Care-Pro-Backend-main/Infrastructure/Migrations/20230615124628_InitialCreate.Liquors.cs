using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateLiquors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Liquors",
                columns: table => new
                {
                    LiquorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LiquorDetails = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LiquorTime = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Liquors", x => x.LiquorID);
                    table.ForeignKey(
                        name: "FK_Liquors_Partograph_PartographID",
                        column: x => x.PartographID,
                        principalTable: "Partograph",
                        principalColumn: "PartographID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Liquors_PartographID",
                table: "Liquors",
                column: "PartographID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Liquors");
        }
    }
}
