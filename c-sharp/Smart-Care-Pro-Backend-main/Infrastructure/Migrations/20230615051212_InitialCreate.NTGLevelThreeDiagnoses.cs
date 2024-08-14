using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialCreateNTGLevelThreeDiagnoses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NTGLevelThreeDiagnoses",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ICDId = table.Column<int>(type: "int", nullable: true),
                    ClinicalFeatures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecommendedInvestigations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvestigationNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreatmentNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pharmacy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Complications = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Prevention = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NTGLevelTwoId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_NTGLevelThreeDiagnoses", x => x.Oid);
                    table.ForeignKey(
                        name: "FK_NTGLevelThreeDiagnoses_NTGLevelTwoDiagnoses_NTGLevelTwoId",
                        column: x => x.NTGLevelTwoId,
                        principalTable: "NTGLevelTwoDiagnoses",
                        principalColumn: "Oid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NTGLevelThreeDiagnoses_NTGLevelTwoId",
                table: "NTGLevelThreeDiagnoses",
                column: "NTGLevelTwoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NTGLevelThreeDiagnoses");
        }
    }
}
