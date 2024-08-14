using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateSkinPreparations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkinPreparation",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeofAnesthesia = table.Column<byte>(type: "tinyint", nullable: false),
                    IsPovidoneIodineUsed = table.Column<bool>(type: "bit", nullable: false),
                    RegionalType = table.Column<byte>(type: "tinyint", nullable: false),
                    LocalMedicineType = table.Column<byte>(type: "tinyint", nullable: false),
                    LevelOfAnesthesia = table.Column<byte>(type: "tinyint", nullable: false),
                    MedicineUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    AnestheticPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterType = table.Column<byte>(type: "tinyint", nullable: false),
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
                    table.PrimaryKey("PK_SkinPreparation", x => x.InteractionId);
                    table.ForeignKey(
                        name: "FK_SkinPreparation_AnestheticPlans_AnestheticPlanId",
                        column: x => x.AnestheticPlanId,
                        principalTable: "AnestheticPlans",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkinPreparation_AnestheticPlanId",
                table: "SkinPreparation",
                column: "AnestheticPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkinPreparation");
        }
    }
}
