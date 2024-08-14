using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateOxytocins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Oxytocins",
                columns: table => new
                {
                    OxytocinID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OxytocinDetails = table.Column<int>(type: "int", nullable: false),
                    OxytocinTime = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Oxytocins", x => x.OxytocinID);
                    table.ForeignKey(
                        name: "FK_Oxytocins_Partograph_PartographID",
                        column: x => x.PartographID,
                        principalTable: "Partograph",
                        principalColumn: "PartographID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Oxytocins_PartographID",
                table: "Oxytocins",
                column: "PartographID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Oxytocins");
        }
    }
}
